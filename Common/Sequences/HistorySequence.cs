using System.Collections;

namespace Common.Sequences
{
    public class HistorySequence<T> : IHistorySequence<T>
    {
        protected SortedList<DateTime, History<T>> _series;

        public SequenceOptions Options { get; set; }

        public HistorySequence(Dictionary<DateTime, History<T>> values)
            : this(new SequenceOptions(), values)
        {
        }

        public HistorySequence(SequenceOptions options, Dictionary<DateTime, History<T>> values)
        {
            Options = options;
            _series = new SortedList<DateTime, History<T>>(values);
        }

        public HistorySequence(Dictionary<DateTime, Dictionary<DateTime, T>> values)
            : this(new SequenceOptions(), values)
        {
        }

        public HistorySequence(SequenceOptions options, Dictionary<DateTime, Dictionary<DateTime, T>> values)
            : this(options, values.ToDictionary(x => x.Key, x => new History<T>(x.Value)))
        {
        }

        public void Add(DateTime refDate, DateTime knownFrom, T value)
            => AddOrUpdate(refDate, knownFrom, value);

        public void AddOrUpdate(DateTime refDate, DateTime knownFrom, T tValue)
        {
            if (_series.TryGetValue(refDate, out History<T> value))
            {
                value.AddOrUpdate(knownFrom, tValue);
            }
            else
            {
                _series.Add(refDate, new History<T>(knownFrom, tValue));
            }
        }

        public void Add(DateTime refDate, Dictionary<DateTime, T> values)
            => AddOrUpdate(refDate, values);

        public void AddOrUpdate(DateTime refDate, Dictionary<DateTime, T> values)
        {
            ArgumentNullException.ThrowIfNull(values);

            foreach (KeyValuePair<DateTime, T> value in values)
            {
                AddOrUpdate(refDate, value.Key, value.Value);
            }
        }

        public bool ContainsKey(DateTime refDate) => _series.ContainsKey(refDate);

        public History<T> this[DateTime refDate] => _series.TryGetValue(refDate, out History<T> value) ? value : null;

        public T this[DateTime refDate, DateTime knownFrom]
        {
            get
            {
                if (_series.TryGetValue(refDate, out History<T> hist))
                {
                    return hist.GetValueKnownOn(knownFrom);
                }

                switch (Options.MissingData)
                {
                    case MissingDataBehaviour.DefaultValue:
                        return default;
                    case MissingDataBehaviour.BackToLast:
                        {
                            IEnumerable<DateTime> dates = _series.Keys.Where(x => x < refDate && _series[x].HasValueKnownOn(knownFrom)).OrderByDescending(x => x);
                            if (dates.Any())
                            {
                                return _series[dates.First()].GetValueKnownOn(knownFrom);
                            }
                        }
                        throw new KeyNotFoundException($"The given key '{refDate}' was not present in the dictionary, and no previous values found.");
                    case MissingDataBehaviour.ForwardToNext:
                        {
                            IEnumerable<DateTime> dates = Keys.Where(x => x > refDate && _series[x].HasValueKnownOn(knownFrom)).OrderBy(x => x);
                            if (dates.Any())
                            {
                                return _series[dates.First()].GetValueKnownOn(knownFrom);
                            }
                        }
                        throw new KeyNotFoundException($"The given key '{refDate}' was not present in the dictionary, and no future values found.");
                    case MissingDataBehaviour.ThrowException:
                    default:
                        throw new KeyNotFoundException($"The given key '{refDate}' was not present in the dictionary.");
                };
            }
        }

        public DateTime? FirstKey => _series.Count == 0 ? null : _series.First().Key;

        public DateTime? LastKey => _series.Count == 0 ? null : _series.Last().Key;

        public int Length => _series.Count;

        public IEnumerable<DateTime> Keys => _series.Keys;

        public IEnumerable<History<T>> Values => _series.Values;

        public IHistorySequence<T> ShiftDates(TimeSpan shift)
        {
            SequenceOptions options = Options.Clone();
            Dictionary<DateTime, Dictionary<DateTime, T>> result =
                _series.AsParallel()
                       .Select(x => new KeyValuePair<DateTime, Dictionary<DateTime, T>>(x.Key.Add(shift), x.Value.ToDictionary()))
                       .ToDictionary(x => x.Key, x => x.Value);
            return new HistorySequence<T>(options, result);
        }

        public IHistorySequence<T> Filter(Func<T, bool> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            Dictionary<DateTime, Dictionary<DateTime, T>> data =
                _series.Where(x => x.Value.Values.All(filter))
                       .ToDictionary(x => x.Key, x => x.Value.ToDictionary().Where(v => filter(v.Value)).ToDictionary(v => v.Key, v => v.Value));
            return new HistorySequence<T>(Options.Clone(), data);
        }

        public IHistorySequence<S> Transform<S>(Func<T, S> transformer)
        {
            ArgumentNullException.ThrowIfNull(transformer);

            Dictionary<DateTime, Dictionary<DateTime, S>> data =
                _series.ToDictionary(
                    x => x.Key,
                    x => x.Value.ToDictionary(transformer));
            return new HistorySequence<S>(Options.Clone(), data);
        }

        public IHistorySequence<T> InRange(DateTime startDate, DateTime endDate)
        {
            Dictionary<DateTime, History<T>> data =
                _series.Where(x => x.Key >= startDate && x.Key <= endDate)
                       .ToDictionary(x => x.Key, x => x.Value);
            return new HistorySequence<T>(Options.Clone(), data);
        }

        public IDateSequence<T> GetSequenceKnownOnDate(DateTime date)
        {
            Dictionary<DateTime, T> data =
                _series.Where(x => x.Value.HasValueKnownOn(date))
                       .ToDictionary(x => x.Key, x => x.Value.GetValueKnownOn(date));
            return new DateSequence<T>(Options.Clone(), data);
        }

        public IDateSequence<T> GetLatestSequence() => GetSequenceKnownOnDate(DateTime.MaxValue);

        public IEnumerable<T> Flatten() => _series.SelectMany(x => x.Value.Values);

        public IEnumerator<KeyValuePair<DateTime, History<T>>> GetEnumerator() => _series.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
