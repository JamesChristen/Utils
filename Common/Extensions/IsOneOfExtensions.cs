using System.Collections.Generic;

namespace System
{
    public static class IsOneOfExtensions
    {
        /// <summary>
        /// Returns true if the source is contained in the <paramref name="options"/>, subject to source.Equals
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsOneOf<T>(this T t, params T[] options)
        {
            ArgumentNullException.ThrowIfNull(t, $"Parameter cannot be null");

            if (options == null)
            {
                return false;
            }

            foreach (T option in options)
            {
                if (t.Equals(option))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the source is contained in the <paramref name="options"/>, subject to <paramref name="comparer"/>.Equals
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsOneOf<T>(this T t, IEqualityComparer<T> comparer, params T[] options)
        {
            ArgumentNullException.ThrowIfNull(t, $"Parameter cannot be null");

            if (options == null)
            {
                return false;
            }

            foreach (T option in options)
            {
                if (comparer.Equals(t, option))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
