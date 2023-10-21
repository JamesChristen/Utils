using System;
using System.Collections.Generic;

namespace Common
{
    public static class MinMax
    {
        public static T Max<T>(T first, T second)
        {
            return Max(first, second, Comparer<T>.Default);
        }

        public static T Max<T>(T first, T second, Comparer<T> comparer)
        {
            return comparer.Compare(first, second) >= 0 ? first : second;
        }

        public static T Min<T>(T first, T second)
        {
            return Min(first, second, Comparer<T>.Default);
        }

        public static T Min<T>(T first, T second, Comparer<T> comparer)
        {
            return comparer.Compare(first, second) <= 0 ? first : second;
        }

        public static DateTime? Max(DateTime? first, DateTime? second)
        {
            if (!first.HasValue && !second.HasValue)
            {
                return null;
            }
            else if (!first.HasValue)
            {
                return second;
            }
            else if (!second.HasValue)
            {
                return first;
            }
            else
            {
                return first.Value >= second.Value ? first : second;
            }
        }

        public static DateTime? Min(DateTime? first, DateTime? second)
        {
            if (!first.HasValue && !second.HasValue)
            {
                return null;
            }
            else if (!first.HasValue)
            {
                return second;
            }
            else if (!second.HasValue)
            {
                return first;
            }
            else
            {
                return first.Value <= second.Value ? first : second;
            }
        }
    }
}
