using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Represents a date range using <see cref="DateTime"/> as the <see cref="Start"/> and <see cref="End"/>
    /// </summary>
    public class DateInterval
    {
        /// <summary>
        /// Returns a DateInterval from <see cref="DateTime.MaxValue"/> to <see cref="DateTime.MinValue"/>
        /// </summary>
        public static DateInterval Max => new DateInterval(DateTime.MinValue, DateTime.MaxValue);

        private DateTime _start;
        public DateTime Start { get => _start; set { SetDates(() => _start = value); } }

        private DateTime _end;
        public DateTime End { get => _end; set { SetDates(() => _end = value); } }

        public DateInterval(DateTime start, DateTime end)
        {
            if (start > end)
            {
                _start = end;
                _end = start;
            }
            else
            {
                _start = start;
                _end = end;
            }
        }

        private void SetDates(Action setDate)
        {
            setDate();
            if (Start > End)
            {
                DateTime temp = _start;
                _start = _end;
                _end = temp;
            }
        }

        /// <summary>
        /// Returns whether <paramref name="date"/> is contained in the interval, inclusive of the <see cref="Start"/> and <see cref="End"/>
        /// </summary>
        public bool Contains(DateTime date)
        {
            return date >= Start && date <= End;
        }

        /// <summary>
        /// Returns whether <paramref name="interval"/> intersects with this interval, optionally inclusive of the <see cref="Start"/> and <see cref="End"/>
        /// </summary>
        public bool Intersects(DateInterval interval, bool includeEdges = false)
        {
            if (includeEdges)
            {
                return Contains(interval.Start) || Contains(interval.End);
            }

            if (interval.Start < End)
            {
                return interval.End > Start;
            }

            if (interval.End > Start)
            {
                return interval.Start < End;
            }

            return false;
        }

        /// <summary>
        /// Returns all days (weekends optional) between <see cref="Start"/> and <see cref="End"/> inclusive
        /// </summary>
        public IEnumerable<DateTime> GetDates(bool includeWeekends = false, bool keepTimeStamp = true)
        {
            return Start.RangeTo(End, includeWeekends, keepTimeStamp);
        }

        /// <summary>
        /// Returns bars of length <paramref name="barTime"/> between <paramref name="startTime"/> and <paramref name="endTime"/> 
        /// (weekends optional) between <see cref="Start"/> and <see cref="End"/> inclusive
        /// </summary>
        public IEnumerable<DateTime> GetBars(TimeSpan barTime, TimeSpan startTime, TimeSpan endTime, bool includeWeekends = false)
        {
            return Start.BarRangeTo(End, barTime, startTime, endTime, includeWeekends);
        }

        public override string ToString() => $"{Start:G} - {End:G}";
    }
}
