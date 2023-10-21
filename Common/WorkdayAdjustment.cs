using System;
using System.Collections.Generic;

namespace Common
{
    public enum WorkdayAdjustment
    {
        None, Before, After
    }

    public static class WorkdayAdjustmentExtensions
    {
        public static DateTime ApplyToDate(this WorkdayAdjustment adj, DateTime date, HashSet<DateTime> holidays = null)
        {
            holidays ??= new HashSet<DateTime>();
            return adj switch
            {
                WorkdayAdjustment.After => date.NextBusinessDayOrDate(holidays),
                WorkdayAdjustment.Before => date.LastBusinessDayOrDate(holidays),
                WorkdayAdjustment.None => date,
                _ => throw new NotImplementedException($"Unsupported {nameof(WorkdayAdjustment)}: {adj}")
            };
        }

        public static int Modifier(this WorkdayAdjustment adj)
        {
            return adj switch
            {
                WorkdayAdjustment.After => 1,
                WorkdayAdjustment.Before => -1,
                WorkdayAdjustment.None => 0,
                _ => throw new NotImplementedException($"Unsupported {nameof(WorkdayAdjustment)}: {adj}")
            };
        }

        public static WorkdayAdjustment Parse(this char c)
        {
            return c switch
            {
                '+' => WorkdayAdjustment.After,
                '-' => WorkdayAdjustment.Before,
                '0' => WorkdayAdjustment.None,
                _ => throw new ArgumentException($"Invalid {nameof(WorkdayAdjustment)}: {c}. Accepted options: +,-,0")
            };
        }

        public static bool CanParse(this char c, out WorkdayAdjustment? adj)
        {
            try
            {
                adj = c.Parse();
                return true;
            }
            catch
            {
                adj = null;
                return false;
            }
        }

        public static char ToChar(this WorkdayAdjustment adj)
        {
            return adj switch
            {
                WorkdayAdjustment.After => '+',
                WorkdayAdjustment.Before => '-',
                WorkdayAdjustment.None => '0',
                _ => throw new NotImplementedException($"Unsupported {nameof(WorkdayAdjustment)} conversion")
            };
        }

        public static bool CanChar(this WorkdayAdjustment adj, out char? c)
        {
            try
            {
                c = adj.ToChar();
                return true;
            }
            catch
            {
                c = null;
                return false;
            }
        }
    }
}
