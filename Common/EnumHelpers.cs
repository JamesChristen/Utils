using System;
using System.Collections.Generic;

namespace Common
{
    public static class EnumHelpers
    {
        public static IEnumerable<T> All<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static bool IsDefined<T>(string value, bool ignoreCase = true) where T : struct, Enum
        {
            return Enum.TryParse<T>(value, ignoreCase, out _);
        }

        public static T Parse<T>(string value, bool ignoreCase = true) where T : struct, Enum
        {
            if (Enum.TryParse(value, ignoreCase, out T result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse {value} as {typeof(T).Name}");
        }

        public static T? ParseNullable<T>(string value, bool ignoreCase = true) where T : struct, Enum
        {
            if (Enum.TryParse(value, ignoreCase, out T result))
            {
                return result;
            }
            return null;
        }
    }
}
