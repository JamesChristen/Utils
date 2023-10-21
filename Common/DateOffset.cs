using System.Text.RegularExpressions;

namespace Common
{
    public class DateOffset
    {
        private readonly List<Offset> _offsets;

        internal int OffsetCount => _offsets.Count;
        internal string[] Offsets => _offsets.Select(x => x.ToString()).ToArray();

        public DateOffset(string offset)
        {
            DateOffset created = Parse(offset);
            _offsets = created._offsets;
        }

        public static implicit operator DateOffset(string offset)
        {
            return new DateOffset(offset);
        }

        public static implicit operator string(DateOffset offset)
        {
            return offset.ToString();
        }

        private DateOffset(IEnumerable<Offset> offsets)
        {
            _offsets = offsets.ToList();
        }

        public DateTime ApplyToDate(DateTime date) => ApplyToDate(date, new HashSet<DateTime>());

        public DateTime ApplyToDate(DateTime date, HashSet<DateTime> holidays)
        {
            DateTime result = date;
            foreach (Offset offset in _offsets)
            {
                result = offset.ApplyToDate(result, holidays);
            }
            return result;
        }

        public override string ToString()
        {
            return string.Join(string.Empty, _offsets.Select(x => x.ToString()));
        }

        internal enum Period
        {
            Day, Week, Month
        }

        internal readonly struct Offset
        {
            public Period Period { get; }
            public int Num { get; }
            public WorkdayAdjustment Adj { get; }

            public Offset(Period period, int num, WorkdayAdjustment adj)
            {
                if (adj == WorkdayAdjustment.None)
                {
                    adj = WorkdayAdjustment.After;
                    // TODO: investigate
                    //throw new ArgumentException($"Direction needs to be explicit, prepend period with + or -");
                }

                if (num < 0)
                {
                    throw new ArgumentException($"Num has to be greater than zero (or equal to zero for {nameof(Period)}.{Period.Day})");
                }

                if (num == 0 && period != Period.Day)
                {
                    throw new ArgumentException($"Zero length period can only be used with {nameof(Period)}.{Period.Day}");
                }

                Period = period;
                Num = num;
                Adj = adj;
            }

            public DateTime ApplyToDate(DateTime date, params DateTime[] holidays)
            {
                return ApplyToDate(date, new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()));
            }

            public DateTime ApplyToDate(DateTime date, HashSet<DateTime> holidays)
            {
                int modifier = Adj.Modifier();
                switch (Period)
                {
                    case Period.Day:
                        if (Num == 0)
                        {
                            return modifier > 0 ? date.NextBusinessDayOrDate(holidays) : date.LastBusinessDayOrDate(holidays);
                        }
                        return date.AddBusinessDays(modifier * Num, holidays);
                    case Period.Week:
                        return date.AddWeekdays(modifier * 5 * Num);
                    case Period.Month:
                        return date.AddMonths(modifier * Num);
                    default:
                        throw new NotImplementedException($"Unsupported {nameof(Period)}: {Period}");
                }
            }

            public override string ToString()
            {
                string period = Period switch
                {
                    Period.Day => "B",
                    Period.Week => "W",
                    Period.Month => "M",
                    _ => throw new NotImplementedException($"Unsupported {nameof(Period)}: {Period}")
                };
                return $"{Adj.ToChar()}{Num}{period}";
            }
        }

        private const string _regexPattern = @"^([+-]?[0-9]+[BWM])?([+-][0-9]+[BWM])*$";
        private static readonly char[] _allowedCodes = new char[] { 'B', 'W', 'M' };
        private static readonly Regex _regex = new Regex("^([+-]?[0-9]+[BWM])?([+-][0-9]+[BWM])*$");

		public static DateOffset Parse(string offset)
        {
            offset = offset?.ToUpper().RemoveWhiteSpace() ?? string.Empty;

            string lettersOnly = new(offset.Where(char.IsLetter).ToArray());
            if (lettersOnly.Except(_allowedCodes).Any())
            {
                throw new ArgumentException($"Cannot parse {nameof(DateOffset)} - only support the following codes: {string.Join(",", _allowedCodes)}");
            }

            List<Offset> result = new();
            int prevStart = 0;
            for (int i = 0; i < offset.Length; i++)
            {
                char currChar = offset[i];
                if (_allowedCodes.Contains(currChar))
                {
                    string substr = offset.Substring(prevStart, i - prevStart + 1);
                    if (!_regex.IsMatch(substr))
                    {
                        throw new ArgumentException($"Invalid format for period ({substr}). Format required: {_regexPattern}");
                    }

                    bool hasExplicitAdjustment = substr[0].IsOneOf('+', '-');
                    WorkdayAdjustment adj = hasExplicitAdjustment ? substr[0].Parse() : WorkdayAdjustment.After;
                    if (!int.TryParse(hasExplicitAdjustment ? substr[1..^1] : substr[0..^1], out int num))
                    {
                        throw new ArgumentException($"Cannot parse length for period {currChar} (Substring: {substr})");
                    }

                    Period period = currChar switch
                    {
                        'B' => Period.Day,
                        'W' => Period.Week,
                        'M' => Period.Month,
                        _ => throw new NotImplementedException($"Unsupported period type: {currChar} (Substring: {substr})")
                    };

                    result.Add(new Offset(period, num, adj));
                    prevStart = i + 1;
                }
            }

            return new DateOffset(result);
        }

        public static bool CanParse(string offset, out DateOffset dateOffset)
        {
            if (_regex.IsMatch(offset?.ToUpper().RemoveWhiteSpace() ?? string.Empty))
            {
                try
                {
                    dateOffset = new DateOffset(offset);
                    return true;
                }
                catch
                {
                    dateOffset = null;
                    return false;
                }
            }
            else
            {
                dateOffset = null;
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is DateOffset off
                || obj is string s && CanParse(s, out off))
            {
                return off.ToString() == ToString();
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(DateOffset o1, DateOffset o2)
        {
            if (o1 is null && o2 is not null
                || (o1 is not null && o2 is null))

            {
                return false;
            }
            return o1 is null && o2 is null || o1.Equals(o2);
        }

        public static bool operator !=(DateOffset o1, DateOffset o2)
        {
            return !(o1 == o2);
        }
	}
}
