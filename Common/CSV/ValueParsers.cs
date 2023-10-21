namespace Common.CSV
{
    public static class ValueParsers
    {
        public static T GetParsedValue<T>(string value)
        {
            if (_parsers.TryGetValue(typeof(T), out Func<string, object> parser))
            {
                return (T)parser(value);
            }
            else if (typeof(T).IsEnum)
            {
                throw new NotImplementedException($"Cannot parse '{value}' to {typeof(T)} directly, use {nameof(ParseEnum)} method directly");
            }
            throw new NotImplementedException($"Cannot parse '{value}' to type {typeof(T).Name} - no parser found");
        }

        public static dynamic GetParsedValue(string value, Type type)
        {
            if (_parsers.TryGetValue(type, out Func<string, object> parser))
            {
                return parser(value);
            }
            else if (type.IsEnum)
            {
                throw new NotImplementedException($"Cannot parse '{value}' to {type.Name} directly, use {nameof(ParseEnum)} method directly");
            }
            throw new NotImplementedException($"Cannot parse '{value}' to type {type.Name} - no parser found");
        }

        private static readonly Dictionary<Type, Func<string, object>> _parsers = new Dictionary<Type, Func<string, object>>
        {
            { typeof(string), s => s },

            { typeof(bool), s => ParseBool(s) },
            { typeof(bool?), s => ParseNullableBool(s) },

            { typeof(byte), s => ParseByte(s) },
            { typeof(byte?), s => ParseNullableByte(s) },

            { typeof(char), s => ParseChar(s) },
            { typeof(char?), s => ParseNullableChar(s) },

            { typeof(DateTime), s => ParseDateTime(s) },
            { typeof(DateTime?), s => ParseNullableDateTime(s) },

            { typeof(decimal), s => ParseDecimal(s) },
            { typeof(decimal?), s => ParseNullableDecimal(s) },

            { typeof(double), s => ParseDouble(s) },
            { typeof(double?), s => ParseNullableDouble(s) },

            { typeof(float), s => ParseFloat(s) },
            { typeof(float?), s => ParseNullableFloat(s) },

            { typeof(int), s => ParseInt(s) },
            { typeof(int?), s => ParseNullableInt(s) },

            { typeof(long), s => ParseLong(s) },
            { typeof(long?), s => ParseNullableLong(s) },

            { typeof(short), s => ParseShort(s) },
            { typeof(short?), s => ParseNullableShort(s) },
        };

        public static bool ParseBool(string value)
        {
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse '{value}' as bool");
        }

        public static bool? ParseNullableBool(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseBool(value);
        }

        public static byte ParseByte(string value)
        {
            if (byte.TryParse(value, out byte result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse '{value}' as byte");
        }

        public static byte? ParseNullableByte(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseByte(value);
        }

        public static char ParseChar(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length != 1)
            {
                throw new ArgumentException($"Cannot parse {value} as char");
            }
            return value[0];
        }

        public static char? ParseNullableChar(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return ParseChar(value);
        }

        public static DateTime ParseDateTime(string value)
        {
            if (DateTime.TryParse(value, out DateTime date))
            {
                return date;
            }
            throw new ArgumentException($"Cannot parse '{value}' as DateTime");
        }

        public static DateTime? ParseNullableDateTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseDateTime(value);
        }

        public static decimal ParseDecimal(string value)
        {
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse '{value}' as decimal");
        }

        public static decimal? ParseNullableDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseDecimal(value);
        }

        public static double ParseDouble(string value)
        {
            if (double.TryParse(value, out double result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse '{value}' as double");
        }

        public static double? ParseNullableDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseDouble(value);
        }

        public static float ParseFloat(string value)
        {
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse '{value}' as float");
        }

        public static float? ParseNullableFloat(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseFloat(value);
        }

        public static int ParseInt(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse '{value}' as int");
        }

        public static int? ParseNullableInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseInt(value);
        }

        public static long ParseLong(string value)
        {
            if (long.TryParse(value, out long longValue))
            {
                return longValue;
            }
            throw new ArgumentException($"Cannot parse '{value}' as long");
        }

        public static long? ParseNullableLong(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseLong(value);
        }

        public static short ParseShort(string value)
        {
            if (short.TryParse(value, out short result))
            {
                return result;
            }
            throw new ArgumentException($"Cannot parse '{value}' as short");
        }

        public static short? ParseNullableShort(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseShort(value);
        }

        public static T ParseEnum<T>(string value) where T : struct, Enum
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                object toParse = value;

                if (int.TryParse(value, out int val))
                {
                    toParse = val;
                }

                if (Enum.IsDefined(typeof(T), toParse))
                {
                    return (T)Enum.Parse(typeof(T), value);
                }
            }
            throw new ArgumentException($"Cannot parse '{value}' as {typeof(T).Name}");
        }

        public static T? ParseNullableEnum<T>(string value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return ParseEnum<T>(value);
        }
    }
}
