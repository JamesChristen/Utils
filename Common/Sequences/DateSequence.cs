using System.Collections;
using System.Collections.Concurrent;

namespace Common.Sequences
{
    public class DateSequence<T> : IDateSequence<T>
    {
        protected SortedList<DateTime, T> _series;

        public SequenceOptions Options { get; set; }

        public DateSequence()
            : this(new SequenceOptions(), new Dictionary<DateTime, T>())
        {
        }

        public DateSequence(Dictionary<DateTime, T> items)
            : this(new SequenceOptions(), items ?? new Dictionary<DateTime, T>())
        {
        }

        public DateSequence(SequenceOptions options)
            : this(options ?? new SequenceOptions(), new Dictionary<DateTime, T>())
        {
        }

        public DateSequence(SequenceOptions options, Dictionary<DateTime, T> items)
        {
            Options = options ?? new SequenceOptions();
            _series = new SortedList<DateTime, T>(items ?? new Dictionary<DateTime, T>());
            AutoFillIfNeeded();
        }

        public IDateSequence<S> CreateWithSameOptions<S>()
        {
            return new DateSequence<S>(Options.Clone());
        }

        public void Add(DateTime key, T value)
        {
            ValidateDate(key);
            _series.Add(key, value);
            AutoFillIfNeeded();
        }

        public void AddOrUpdate(DateTime key, T value)
        {
            ValidateDate(key);
            _series[key] = value;
            AutoFillIfNeeded();
        }

        public void AddRange(Dictionary<DateTime, T> range)
        {
            ValidateDates(range.Keys);

            IEnumerable<DateTime> commonDates = _series.Keys.Intersect(range.Keys);
            if (commonDates.Any())
            {
                throw new ArgumentException($"Range has overlapping keys with existing series");
            }

            foreach (KeyValuePair<DateTime, T> pair in range)
            {
                Add(pair.Key, pair.Value);
            }

            AutoFillIfNeeded();
        }

        public void AddOrUpdateRange(Dictionary<DateTime, T> range)
        {
            ValidateDates(range.Keys);

            foreach (KeyValuePair<DateTime, T> pair in range)
            {
                _series[pair.Key] = pair.Value;
            }

            AutoFillIfNeeded();
        }

        public T this[DateTime date]
        {
            get
            {
                if (_series.TryGetValue(date, out T value))
                {
                    return value;
                }
                switch (Options.MissingData)
                {
                    case MissingDataBehaviour.DefaultValue: 
                        return default;
                    case MissingDataBehaviour.BackToLast:
                        {
                            IEnumerable<DateTime> dates = Keys.Where(x => x < date).OrderByDescending(x => x);
                            if (dates.Any())
                            {
                                return _series[dates.First()];
                            }
                        }
                        throw new KeyNotFoundException($"The given key '{date}' was not present in the dictionary, and no previous values found.");
                    case MissingDataBehaviour.ForwardToNext:
                        {
                            IEnumerable<DateTime> dates = Keys.Where(x => x > date).OrderBy(x => x);
                            if (dates.Any())
                            {
                                return _series[dates.First()];
                            }
                        }
                        throw new KeyNotFoundException($"The given key '{date}' was not present in the dictionary, and no future values found.");
                    case MissingDataBehaviour.ThrowException:
                    default:
                        throw new KeyNotFoundException($"The given key '{date}' was not present in the dictionary.");
                };
            }
        }

        public T GetValueOrDefault(DateTime key, T defaultValue = default) => _series.TryGetValue(key, out T value) ? value : defaultValue;

        public T FirstValueOrDefault(T defaultValue = default) => _series.Count == 0 ? defaultValue : _series.Values.First();

        public T LastValueOrDefault(T defaultValue = default) => _series.Count == 0 ? defaultValue : _series.Values.Last();

        public DateTime? FirstKey => _series.Count == 0 ? null : _series.Keys.First();

        public DateTime? LastKey => _series.Count == 0 ? null : _series.Keys.Last();

        public int Length => _series.Count;

        public bool IsEmpty => _series.Count == 0;

        public IEnumerable<DateTime> Keys => _series.Keys;

        public IEnumerable<T> Values => _series.Values;

        public bool ContainsKey(DateTime key) => _series.ContainsKey(key); 

        public bool ContainsKeyOrDefault(DateTime key)
        {
            if (ContainsKey(key))
            {
                return true;
            }
            else if (Options.MissingData == MissingDataBehaviour.DefaultValue)
            {
                return false;
            }
            else
            {
                try
                {
                    _ = this[key];
                    return true;
                }
                catch (KeyNotFoundException)
                {
                    return false;
                }
            }

        }

        public IEnumerator<KeyValuePair<DateTime, T>> GetEnumerator() => _series.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual void Print(Action<string> print, string separator = "\r\n")
        {
            string output = string.Join(separator, _series.Select(x => $"{x.Key:G} => {x.Value?.ToString() ?? "null"}"));
            print(output);
        }

        public virtual bool SequenceEqual(IDateSequence<T> other)
        {
            if (!Keys.SequenceEqual(other.Keys))
            {
                return false;
            }
            foreach (DateTime key in Keys)
            {
                if (!this[key].Equals(other[key]))
                {
                    return false;
                }
            }
            return true;
        }

        public IDateSequence<T> SetStart(DateTime date)
        {
            ValidateDate(date);

            if ((FirstKey ?? DateTime.MaxValue) > date)
            {
                _series[date] = default;
            }
            else
            {
                if (!_series.ContainsKey(date))
                {
                    _series[date] = default;
                }
                _series = new SortedList<DateTime, T>(_series.Where(x => x.Key >= date).ToDictionary(x => x.Key, x => x.Value));
            }

            AutoFillIfNeeded();
            return this;
        }

        public IDateSequence<T> SetEnd(DateTime date)
        {
            ValidateDate(date);

            if ((LastKey ?? DateTime.MinValue) < date)
            {
                _series[date] = default;
            }
            else
            {
                if (!_series.ContainsKey(date))
                {
                    _series[date] = default;
                }
                _series = new SortedList<DateTime, T>(_series.Where(x => x.Key <= date).ToDictionary(x => x.Key, x => x.Value));
            }

            AutoFillIfNeeded();
            return this;
        }

        public IDateSequence<T> InRange(DateTime startDate, DateTime endDate)
        {
            return new DateSequence<T>(_series.Where(x => x.Key >= startDate && x.Key <= endDate).ToDictionary(x => x.Key, x => x.Value));
        }

        public IDateSequence<T> FillMissingDates()
        {
            if (_series.Count <= 1)
            {
                return this;
            }

            IEnumerable<DateTime> dates = Options.Frequency switch
            {
                SequenceFrequency.Daily => FirstKey.Value.RangeTo(LastKey.Value, includeWeekends: Options.IncludeWeekends, keepTimeStamp: true),
                SequenceFrequency.Intraday => FirstKey.Value.BarRangeTo(LastKey.Value, Options.BarTime, Options.BarStart, Options.BarEnd, Options.IncludeWeekends),
                _ => throw new NotImplementedException($"No implementation for filling dates with frequency {nameof(SequenceFrequency)}.{Options.Frequency}")
            };

            Dictionary<DateTime, T> values = new Dictionary<DateTime, T>();
            foreach (DateTime date in dates)
            {
                values[date] = this[date];
            }
            _series = new SortedList<DateTime, T>(values);
            return this;
        }

        internal void ValidateDate(DateTime date)
        {
            if (!DateIsValid(date, out List<string> errors))
            {
                throw new ArgumentException(string.Join(Environment.NewLine, errors));
            }
        }

        internal bool DateIsValid(DateTime date, out List<string> errors)
        {
            errors = new List<string>();

            if (!Options.IncludeWeekends && !date.IsWeekday())
            {
                errors.Add($"{date:d} is weekend and {nameof(SequenceOptions)} specifies no weekends");
            }

            if (Options.Frequency == SequenceFrequency.Daily
                && FirstKey.HasValue
                && date.TimeOfDay != FirstKey.Value.TimeOfDay)
            {
                errors.Add($"{nameof(SequenceOptions)}.{nameof(Options.Frequency)} is set as {SequenceFrequency.Daily.ToLongString()} but TimeOfDay does not match other dates");
            }

            if (Options.Frequency == SequenceFrequency.Intraday
                && (Options.BarStart > date.TimeOfDay || (Options.BarEnd != TimeSpan.Zero && Options.BarEnd < date.TimeOfDay)))
            {
                errors.Add($"TimeSpan of date ({date:T}) falls outside specified range {Options.BarStart:T} => {Options.BarEnd:T}");
            }

            return errors.Count == 0;
        }

        internal void ValidateDates(IEnumerable<DateTime> dates)
        {
            if (!DatesAreValid(dates, out List<string> errors))
            {
                throw new ArgumentException(string.Join(Environment.NewLine, errors));
            }
        }

        internal bool DatesAreValid(IEnumerable<DateTime> dates, out List<string> errors)
        {
            errors = new List<string>();

            if (dates == null || !dates.Any())
            {
                return true;
            }

            bool isValid = true;
            foreach (DateTime d in dates)
            {
                isValid &= DateIsValid(d, out List<string> e);
                errors.AddRange(e);
            }

            return isValid;
        }

        internal void AutoFillIfNeeded()
        {
            if (Options.AutoFillDates)
            {
                FillMissingDates();
            }
        }

        public IDateSequence<T> ShiftDates(TimeSpan shift)
        {
            SequenceOptions options = Options.Clone();
            Dictionary<DateTime, T> result =
                _series.AsParallel()
                       .Select(x => new KeyValuePair<DateTime, T>(x.Key.Add(shift), x.Value))
                       .ToDictionary(x => x.Key, x => x.Value);
            return new DateSequence<T>(options, result);
        }

        public IDateSequence<T> Filter(Func<T, bool> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return new DateSequence<T>(Options.Clone(), this.Where(x => filter(x.Value)).ToDictionary(x => x.Key, x => x.Value));
        }

        public IDateSequence<S> Transform<S>(Func<T, S> transformer)
        {
            ArgumentNullException.ThrowIfNull(transformer);

            SequenceOptions options = Options.Clone();

            ConcurrentDictionary<DateTime, Exception> errors = new ConcurrentDictionary<DateTime, Exception>();

            Dictionary<DateTime, S> result =
                _series.AsParallel()
                       .Select(x =>
                       {
                           try
                           {
                               S value = transformer(x.Value);
                               return new KeyValuePair<DateTime, S>(x.Key, value);
                           }
                           catch (Exception ex)
                           {
                               errors.AddOrUpdate(x.Key, f => ex, (f, y) => ex);
                               return new KeyValuePair<DateTime, S>(x.Key, default);
                           }
                       })
                       .ToDictionary(x => x.Key, x => x.Value);

            if (errors.Any())
            {
                throw new AggregateException(
                    $"{errors.Count} errors transforming data", 
                    errors.Select(x => new Exception($"Error transforming for date {x.Key:G}: {x.Value.Message}", x.Value)));
            }

            return new DateSequence<S>(options, result);
        }

        public IDateSequence<S> Transform<S>(Func<DateTime, T, S> transformer)
        {
            ArgumentNullException.ThrowIfNull(transformer);

            SequenceOptions options = Options.Clone();

            ConcurrentDictionary<DateTime, Exception> errors = new ConcurrentDictionary<DateTime, Exception>();

            Dictionary<DateTime, S> result =
                _series.AsParallel()
                       .Select(x =>
                       {
                           try
                           {
                               S value = transformer(x.Key, x.Value);
                               return new KeyValuePair<DateTime, S>(x.Key, value);
                           }
                           catch (Exception ex)
                           {
                               errors.AddOrUpdate(x.Key, f => ex, (f, y) => ex);
                               return new KeyValuePair<DateTime, S>(x.Key, default);
                           }
                       })
                       .ToDictionary(x => x.Key, x => x.Value);

            if (errors.Any())
            {
                throw new AggregateException(
                    $"{errors.Count} errors transforming data",
                    errors.Select(x => new Exception($"Error transforming for date {x.Key:G}: {x.Value.Message}", x.Value)));
            }

            return new DateSequence<S>(options, result);
        }

        public IDateSequence<S> Join<T2, S>(IDateSequence<T2> seq, JoinOptions<T, T2, S> joinOptions)
        {
            ArgumentNullException.ThrowIfNull(joinOptions);
            ArgumentNullException.ThrowIfNull(seq);

            HashSet<DateTime> dates = new HashSet<DateTime>(Keys.Union(seq.Keys));
            Dictionary<DateTime, S> result = new Dictionary<DateTime, S>(dates.Count);
            foreach (DateTime date in dates)
            {
                S value = joinOptions.GetValue(date, this, seq);
                result[date] = value;
            }
            return new DateSequence<S>(result);
        }

        public IHistorySequence<T> ToHistoryUsingRefDate()
        {
            return ToHistory((d, v) => d);
        }

        public IHistorySequence<T> ToHistoryKnownFrom(DateTime knownFrom)
        {
            return ToHistory((d, v) => knownFrom);
        }

        public IHistorySequence<T> ToHistory(Func<DateTime, T, DateTime> knownFromProvider)
        {
            ArgumentNullException.ThrowIfNull(knownFromProvider);

            return new HistorySequence<T>(
                _series.ToDictionary(
                    x => x.Key,
                    x => new History<T>(knownFromProvider(x.Key, x.Value), x.Value))
                );
        }

        public override string ToString()
        {
            if (Length > 0)
            {
                return $"DateSequence<{typeof(T).Name}> | MinDate: {FirstKey.Value:G} | MaxDate: {LastKey.Value:G} | Count: {Length}";
            }
            return $"DateSequence<{typeof(T).Name}> | Empty";
        }

        public static IDateSequence<T> Empty => new DateSequence<T>();
    }
}
