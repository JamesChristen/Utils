using Common;

namespace System
{
    public static class DateExtensions
    {
        private static DateTime KeepTimeStamp(this DateTime d, bool keep)
        {
            return keep ? d : d.Date;
        }

        #region Weekdays

        /// <summary>
        /// Returns if the day of the week is a weekday
        /// </summary>
        public static bool IsWeekday(this DateTime d)
        {
            return !(d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        /// Adds <paramref name="numDays"/> of weekdays to date. Holiday agnostic
        /// </summary>
        public static DateTime AddWeekdays(this DateTime d, int numDays, bool keepTimeStamp = true)
        {
            int offsetInWeeks = numDays / 5;
            int restInDays = numDays % 5;

            DateTime newDate = d.AddDays(offsetInWeeks * 7);
            for (int i = 1; i <= Math.Abs(restInDays); i++)
            {
                newDate = restInDays > 0 ? newDate.NextWeekday(keepTimeStamp) : newDate.LastWeekday(keepTimeStamp);
            }
            return newDate.KeepTimeStamp(keepTimeStamp);
        }

        /// <summary>
        /// Returns the next weekday. Holiday agnostic
        /// </summary>
        public static DateTime NextWeekday(this DateTime d, bool keepTimeStamp = true)
        {
            DateTime result;

            if (d.DayOfWeek == DayOfWeek.Friday)
            {
                result = d.AddDays(3);
            }
            else if (d.DayOfWeek == DayOfWeek.Saturday)
            {
                result = d.AddDays(2);
            }
            else
            {
                result = d.AddDays(1);
            }

            return result.KeepTimeStamp(keepTimeStamp);
        }

        /// <summary>
        /// Returns same date if it is a weekday, or the following Monday if not. Holiday agnostic
        /// </summary>
        public static DateTime NextWeekdayOrDate(this DateTime d, bool keepTimeStamp = true)
        {
            return d.IsWeekday() ? d.KeepTimeStamp(keepTimeStamp) : d.NextWeekday(keepTimeStamp);
        }

        /// <summary>
        /// Returns the next date based on the <paramref name="freq"/>, with frequencies greater than daily being the end of the next period. Holiday agnostic<br/>
        /// Only Daily, Monthly, Quarterly, and Yearly are currently defined. Assumes <paramref name="d"/> is a correct date in that frequency <paramref name="freq"/>
        /// </summary>
        public static DateTime NextDate(this DateTime d, Frequency freq, bool includeWeekends = false)
        {
            return freq switch
            {
                Frequency.Daily => includeWeekends ? d.AddDays(1) : d.NextWeekday(),
                Frequency.Monthly => d.NextMonthEnd(),
                Frequency.Quarterly => d.NextQuarterEnd(),
                Frequency.Yearly => d.NextYearEnd(),
                _ => throw new NotImplementedException($"No {nameof(NextDate)} implementation for {freq.ToLongString()}"),
            };
        }

        private static DateTime NextMonthEnd(this DateTime d)
        {
            if (d.EndOfMonth() == d) // If end of month return next month
            {
                return d.AddMonths(1).EndOfMonth();
            }
            // Else return end of current month
            return d.EndOfMonth();
        }

        private static DateTime NextQuarterEnd(this DateTime d)
        {
            if (d.Month % 3 == 0 && d.EndOfMonth() == d) // If end of quarter return next quarter
            {
                return d.AddMonths(3).EndOfMonth();
            }
            // Else return end of current quarter
            return d.AddMonths(3 - (d.Month % 3)).EndOfMonth();
        }

        private static DateTime NextYearEnd(this DateTime d)
        {
            if (d.Month == 12 && d.EndOfMonth() == d) // If end of year return next year
            {
                return d.AddYears(1).EndOfMonth();
            }
            // Else return end of current year
            return d.EndOfYear();
        }

        /// <summary>
        /// Returns the previous weekday. Holiday agnostic
        /// </summary>
        public static DateTime LastWeekday(this DateTime d, bool keepTimeStamp = true)
        {
            DateTime result;

            if (d.DayOfWeek == DayOfWeek.Monday)
            {
                result = d.AddDays(-3);
            }
            else if (d.DayOfWeek == DayOfWeek.Sunday)
            {
                result = d.AddDays(-2);
            }
            else
            {
                result = d.AddDays(-1);
            }

            return result.KeepTimeStamp(keepTimeStamp);
        }

        /// <summary>
        /// Returns same date if it is a weekday, or the preceeding Friday if not. Holiday agnostic
        /// </summary>
        public static DateTime LastWeekdayOrDate(this DateTime d, bool keepTimeStamp = true)
        {
            return d.IsWeekday() ? d.KeepTimeStamp(keepTimeStamp) : d.LastWeekday(keepTimeStamp);
        }

        #endregion

        #region Business days

        /// <summary>
        /// Returns whether the date is a weekday and not a holiday
        /// </summary>
        public static bool IsBusinessDay(this DateTime d, HashSet<DateTime> holidays = null)
        {
            return d.IsWeekday() && (holidays == null || !holidays.Contains(d.Date));
        }

        /// <summary>
        /// Returns whether the date is a weekday and not a holiday
        /// </summary>
        public static bool IsBusinessDay(this DateTime d, params DateTime[] holidays)
            => d.IsBusinessDay(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()));

        /// <summary>
        /// Adds <paramref name="numDays"/> of weekdays to date, holiday dependent
        /// </summary>
        public static DateTime AddBusinessDays(this DateTime d, int numDays, HashSet<DateTime> holidays = null, bool keepTimeStamp = true)
        {
            bool forward = numDays >= 0;
            numDays = Math.Abs(numDays);
            while (numDays > 0)
            {
                d = forward ? d.NextBusinessDay(holidays) : d.LastBusinessDay(holidays);
                numDays--;
            }
            return d.KeepTimeStamp(keepTimeStamp);
        }

        /// <summary>
        /// Adds <paramref name="numDays"/> of weekdays to date, holiday dependent
        /// </summary>
        public static DateTime AddBusinessDays(this DateTime d, int numDays, bool keepTimeStamp = true, params DateTime[] holidays)
            => d.AddBusinessDays(numDays, new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()), keepTimeStamp);

        /// <summary>
        /// Returns the next business day, holiday dependent
        /// </summary>
        public static DateTime NextBusinessDay(this DateTime d, HashSet<DateTime> holidays = null, bool keepTimeStamp = true)
        {
            DateTime result;

            if (d.DayOfWeek == DayOfWeek.Friday)
            {
                result = d.AddDays(3);
            }
            else if (d.DayOfWeek == DayOfWeek.Saturday)
            {
                result = d.AddDays(2);
            }
            else
            {
                result = d.AddDays(1);
            }

            if (!result.IsBusinessDay(holidays))
            {
                result = result.NextBusinessDay(holidays);
            }

            return result.KeepTimeStamp(keepTimeStamp);
        }

        /// <summary>
        /// Returns the next business day, holiday dependent
        /// </summary>
        public static DateTime NextBusinessDay(this DateTime d, bool keepTimeStamp = true, params DateTime[] holidays)
            => d.NextBusinessDay(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()), keepTimeStamp);

        /// <summary>
        /// Returns same date if it is a business day, or the following business if not. Holiday dependent
        /// </summary>
        public static DateTime NextBusinessDayOrDate(this DateTime d, HashSet<DateTime> holidays = null, bool keepTimeStamp = true)
        {
            return d.IsBusinessDay(holidays) ? d.KeepTimeStamp(keepTimeStamp) : d.NextBusinessDay(holidays, keepTimeStamp);
        }

        /// <summary>
        /// Returns same date if it is a business day, or the following business day if not. Holiday dependent
        /// </summary>
        public static DateTime NextBusinessDayOrDate(this DateTime d, bool keepTimeStamp = true, params DateTime[] holidays)
            => d.NextBusinessDayOrDate(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()), keepTimeStamp);

        /// <summary>
        /// Returns the previous business day, holiday dependent
        /// </summary>
        public static DateTime LastBusinessDay(this DateTime d, HashSet<DateTime> holidays = null, bool keepTimeStamp = true)
        {
            DateTime result;

            if (d.DayOfWeek == DayOfWeek.Monday)
            {
                result = d.AddDays(-3);
            }
            else if (d.DayOfWeek == DayOfWeek.Sunday)
            {
                result = d.AddDays(-2);
            }
            else
            {
                result = d.AddDays(-1);
            }

            if (!result.IsBusinessDay(holidays))
            {
                result = result.LastBusinessDay(holidays);
            }

            return result.KeepTimeStamp(keepTimeStamp);
        }

        /// <summary>
        /// Returns the previous business day, holiday dependent
        /// </summary>
        public static DateTime LastBusinessDay(this DateTime d, bool keepTimeStamp = true, params DateTime[] holidays)
            => d.LastBusinessDay(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()), keepTimeStamp);

        /// <summary>
        /// Returns same date if it is a business day, or the previous business day if not. Holiday dependent
        /// </summary>
        public static DateTime LastBusinessDayOrDate(this DateTime d, HashSet<DateTime> holidays = null, bool keepTimeStamp = true)
        {
            return d.IsBusinessDay(holidays) ? d.KeepTimeStamp(keepTimeStamp) : d.LastBusinessDay(holidays, keepTimeStamp);
        }

        /// <summary>
        /// Returns same date if it is a business day, or the previous business day if not. Holiday dependent
        /// </summary>
        public static DateTime LastBusinessDayOrDate(this DateTime d, bool keepTimeStamp = true, params DateTime[] holidays)
            => d.LastBusinessDayOrDate(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()), keepTimeStamp);

        /// <summary>
        /// Returns the number of business days between the date (exclusive) and <paramref name="lastDay"/> (inclusive), holiday dependent<br/>
        /// <paramref name="lastDay"/> has to be greater than or equal to 'firstDay'
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
            {
                throw new ArgumentException("Incorrect last day " + lastDay);
            }

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days;
            int fullWeekCount = businessDays / 7;
            
            if (businessDays > fullWeekCount * 7)
            {
                // We are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                {
                    lastDayOfWeek += 7;
                }

                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7) // Both Saturday and Sunday are in the remaining time interval
                    {
                        businessDays -= 2;
                    }
                    else if (lastDayOfWeek >= 6) // Only Saturday is in the remaining time interval
                    {
                        businessDays -= 1;
                    }
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7) // Only Sunday is in the remaining time interval
                {
                    businessDays -= 1;
                }
            }

            // Subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                {
                    --businessDays;
                }
            }

            return businessDays;
        }
        #endregion

        /// <summary>
        /// Returns every day from date to <paramref name="to"/> inclusive, optionally with weekends<br/>
        /// <paramref name="to"/> has to be greater than or equal to date<br/>
        /// If <paramref name="keepTimeStamp"/> is true, date and <paramref name="to"/> have to have the same TimeStamp
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<DateTime> RangeTo(
            this DateTime d, 
            DateTime to, 
            bool includeWeekends = false, 
            bool keepTimeStamp = true,
            Frequency freq = Frequency.Daily)
        {
            if (to < d)
            {
                throw new ArgumentException($"Far date ({to:G}) cannot be before start date ({d:G})");
            }

            if (keepTimeStamp && d.TimeOfDay != to.TimeOfDay)
            {
                throw new ArgumentException($"Far date time ({to:T}) different to start date time ({d:T})");
            }

            for (DateTime date = d; date <= to; date = date.NextDate(freq, includeWeekends))
            {
                yield return keepTimeStamp ? date : date.Date;
            }
        }

        /// <summary>
        /// Returns every day from <paramref name="from"/> to date inclusive, optionally with weekends
        /// </summary>
        public static IEnumerable<DateTime> RangeFrom(
            this DateTime d, 
            DateTime from, 
            bool includeWeekends = false, 
            bool keepTimeStamp = true)
        {
            return from.RangeTo(d, includeWeekends, keepTimeStamp);
        }

        /// <summary>
        /// Returns every day from date to <paramref name="to"/> inclusive, optionally with weekends<br/>
        /// <paramref name="barTime"/> cannot be TimeSpan.Zero<br/>
        /// <paramref name="to"/> has to be greater than or equal to date<br/>
        /// <paramref name="barTime"/> has to make even bars between <paramref name="startTime"/> and <paramref name="endTime"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<DateTime> BarRangeTo(
            this DateTime d, 
            DateTime to, 
            TimeSpan barTime, 
            TimeSpan startTime, 
            TimeSpan endTime, 
            bool includeWeekends = false)
        {
            if (barTime == TimeSpan.Zero)
            {
                throw new ArgumentException($"Bar time cannot be zero");
            }

            if (to < d)
            {
                throw new ArgumentException($"Far date ({to:G}) cannot be before start date ({d:G})");
            }

            if ((to - d).TotalMilliseconds % barTime.TotalMilliseconds != 0)
            {
                throw new ArgumentException($"Bar interval ({barTime:T}) does not make even bars between {d:G} and {to:T}");
            }

            while (d <= to)
            {
                if ((includeWeekends || d.IsWeekday()) && d.TimeOfDay >= startTime && (endTime == TimeSpan.Zero || d.TimeOfDay <= endTime))
                {
                    yield return d;
                }
                d = d.AddMilliseconds(barTime.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Returns every day from date to <paramref name="to"/> inclusive, optionally with weekends<br/>
        /// <paramref name="barTime"/> cannot be TimeSpan.Zero<br/>
        /// <paramref name="from"/> has to be less than or equal to date<br/>
        /// <paramref name="barTime"/> has to make even bars between <paramref name="startTime"/> and <paramref name="endTime"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<DateTime> BarRangeFrom(
            this DateTime d, 
            DateTime from, 
            TimeSpan barTime, 
            TimeSpan startTime, 
            TimeSpan endTime, 
            bool includeWeekends = false)
        {
            return from.BarRangeTo(d, barTime, startTime, endTime, includeWeekends);
        }

        /// <summary>
        /// Returns the first day of the month
        /// </summary>
        public static DateTime StartOfMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1);
        }

        /// <summary>
        /// Returns the last day of the month
        /// </summary>
        public static DateTime EndOfMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Returns the first day of the year
        /// </summary>
        public static DateTime StartOfYear(this DateTime d)
        {
            return new DateTime(d.Year, 1, 1);
        }

        /// <summary>
        /// Returns the last day of the year
        /// </summary>
        public static DateTime EndOfYear(this DateTime d)
        {
            return new DateTime(d.Year, 12, 31);
        }

        /// <summary>
        /// Returns the first business day of the month, holiday dependent
        /// </summary>
        public static DateTime StartOfMonthBusinessDay(this DateTime d, HashSet<DateTime> holidays = null)
        {
            return d.StartOfMonth().NextBusinessDayOrDate(holidays);
        }

        /// <summary>
        /// Returns the first business day of the month, holiday dependent
        /// </summary>
        public static DateTime StartOfMonthBusinessDay(this DateTime d, params DateTime[] holidays)
            => d.StartOfMonthBusinessDay(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()));

        /// <summary>
        /// Returns the last business day of the month, holiday dependent
        /// </summary>
        public static DateTime EndOfMonthBusinessDay(this DateTime d, HashSet<DateTime> holidays = null)
        {
            return d.EndOfMonth().LastBusinessDayOrDate(holidays);
        }

        /// <summary>
        /// Returns the last business day of the month, holiday dependent
        /// </summary>
        public static DateTime EndOfMonthBusinessDay(this DateTime d, params DateTime[] holidays)
            => d.EndOfMonthBusinessDay(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()));

        /// <summary>
        /// Returns the first business day of the year, holiday dependent
        /// </summary>
        public static DateTime StartOfYearBusinessDay(this DateTime d, HashSet<DateTime> holidays = null)
        {
            return d.StartOfYear().NextBusinessDayOrDate(holidays);
        }

        /// <summary>
        /// Returns the first business day of the year, holiday dependent
        /// </summary>
        public static DateTime StartOfYearBusinessDay(this DateTime d, params DateTime[] holidays)
            => d.StartOfYearBusinessDay(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()));

        /// <summary>
        /// Returns the last business day of the year, holiday dependent
        /// </summary>
        public static DateTime EndOfYearBusinessDay(this DateTime d, HashSet<DateTime> holidays = null)
        {
            return d.EndOfYear().LastBusinessDayOrDate(holidays);
        }

        /// <summary>
        /// Returns the last business day of the year, holiday dependent
        /// </summary>
        public static DateTime EndOfYearBusinessDay(this DateTime d, params DateTime[] holidays)
            => d.EndOfYearBusinessDay(new HashSet<DateTime>(holidays ?? Array.Empty<DateTime>()));

        /// <summary>
        /// Returns the next instance of <paramref name="dow"/>
        /// </summary>
        public static DateTime NextDayOfWeek(this DateTime d, DayOfWeek dow)
        {
            int shift = dow - d.DayOfWeek;
            if (shift < 1)
            {
                shift += 7;
            }
            return d.AddDays(shift);
        }

        /// <summary>
        /// Returns date if the day of the week matches <paramref name="dow"/>, or the next instance if not
        /// </summary>
        public static DateTime NextDayOfWeekOrSame(this DateTime d, DayOfWeek dow)
        {
            if (d.DayOfWeek == dow)
            {
                return d;
            }
            return d.NextDayOfWeek(dow);
        }
    }
}
