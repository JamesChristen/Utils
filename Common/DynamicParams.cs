using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common
{
    public class DynamicParams<T> where T : struct, Enum
    {
        private readonly char _keyValueSeparator = ':';
        private readonly char _pairSeparator = ';';
        private readonly Dictionary<T, dynamic> _params;
        private readonly Dictionary<T, T> _mappedParams;

        public static DynamicParams<T> Empty => new DynamicParams<T>();

        public DynamicParams(char keyValueSeparator = ':', char pairSeparator = ';')
            : this(null, keyValueSeparator, pairSeparator)
        {
        }

        public DynamicParams(IDictionary<T, dynamic> @params, char keyValueSeparator = ':', char pairSeparator = ';')
        {
            if (keyValueSeparator == pairSeparator)
            {
                throw new ArgumentException($"DynamicParams: KeyValue and Pair separators cannot be the same");
            }

            _keyValueSeparator = keyValueSeparator;
            _pairSeparator = pairSeparator;
            _params = @params?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<T, dynamic>();
            _mappedParams = new Dictionary<T, T>();
        }

        public DynamicParams(params Tuple<T, dynamic>[] values)
            : this(values.ToDictionary(x => x.Item1, x => x.Item2))
        {
        }

        public int Count => _params.Count;

        public IEnumerable<T> Keys => _params.Keys;

        public Dictionary<T, dynamic> Values => _params;

        /// <summary>
        /// Links <paramref name="key"/> to <paramref name="mappedTo"/> such that requesting <paramref name="key"/> returns the value
        /// stored against <paramref name="mappedTo"/>.<br/>
        /// Adding a map that causes circular references throws an <see cref="ArgumentException"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void MapParam(T key, T mappedTo)
        {
            HashSet<T> mapped = new HashSet<T>
            {
                key
            };

            T next = mappedTo;
            do
            {
                if (mapped.Contains(next))
                {
                    throw new ArgumentException($"Circular mapped reference");
                }
                mapped.Add(next);
            }
            while (_mappedParams.TryGetValue(next, out next));
            _mappedParams[key] = mappedTo;
        }

        /// <summary>
        /// Returns whether <paramref name="key"/> has a value set, or is mapped to a different key with a value.<br/>
        /// <paramref name="allowNull"/> dictates whether a null value is allowed, assuming a value can be found.
        /// </summary>
        public bool HasParam(T key, bool allowNull = true)
        {
            if (TryGetValue(key, out dynamic value))
            {
                return allowNull || value is not null;
            }
            return false;
        }

        /// <summary>
        /// Sets the value for <paramref name="key"/>, overwriting any existing value
        /// </summary>
        public void SetParam<TValue>(T key, TValue value)
        {
            _params[key] = value;
        }

        private bool TryGetValue(T key, out dynamic value)
        {
            if (_params.TryGetValue(key, out value))
            {
                return true;
            }
            else if (_mappedParams.TryGetValue(key, out T mappedKey))
            {
                return TryGetValue(mappedKey, out value);
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Returns the dynamic value for <paramref name="key"/>.<br/>
        /// Throws an <see cref="ArgumentException"/> if the parameter is not found
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public dynamic GetDynamic(T key)
        {
            if (!TryGetValue(key, out dynamic value))
            {
                throw new ArgumentException($"Dictionary<{typeof(T).Name}, dynamic> Params does not contain key '{key}'");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Returns the strongly typed value for <paramref name="key"/>.<br/>
        /// Throws an <see cref="ArgumentException"/> if the parameter is not found.<br/>
        /// Throws an <see cref="ArgumentException"/> if the value is null and <paramref name="allowNull"/> is false.<br/>
        /// Throws an <see cref="ArgumentException"/> if the value is not of the type <typeparamref name="TValue"/>.<br/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public TValue GetParam<TValue>(T key, bool allowNull = true)
        {
            if (!TryGetValue(key, out dynamic value))
            {
                throw new ArgumentException($"Dictionary<{typeof(T).Name}, dynamic> Params does not contain key '{key}'");
            }
            else if (!allowNull && value is null)
            {
                throw new ArgumentException($"Param '{key}' is null");
            }
            else if (value is null)
            {
                return default;
            }
            else if (value is not TValue)
            {
                throw new ArgumentException($"Param '{key}' is not of type '{typeof(TValue).Name}'");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Returns the strongly typed value for <paramref name="key"/> if found, or the <paramref name="defaultValue"/> if not found.<br/>
        /// Throws an <see cref="ArgumentException"/> if the value is not null and not of the type <typeparamref name="TValue"/>.<br/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public TValue GetParamOrDefault<TValue>(T key, TValue defaultValue = default)
        {
            if (!TryGetValue(key, out dynamic value))
            {
                return defaultValue;
            }
            else if (value is not null and not TValue)
            {
                throw new ArgumentException($"Param '{key}' is not of type '{typeof(TValue).Name}'");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Returns whether the params contains all of the params of <paramref name="params"/>
        /// </summary>
        public bool ContentContains(DynamicParams<T> @params)
        {
            return _params.ContentContains(@params._params);
        }


        public override string ToString()
        {
            return _params.ToString(_keyValueSeparator, _pairSeparator);
        }

        public string ToString(DynamicParser parser)
        {
            return string.Join(_pairSeparator, _params.Select(x => $"{x.Key}{_keyValueSeparator}{parser.ToString(x.Key)(x.Value)}"));
        }

        public string ToString(char keySeparator, char pairSeparator)
        {
            return _params.ToString(keySeparator, pairSeparator);
        }

        /// <summary>
        /// Returns the params joined by the <see cref="_keyValueSeparator"/>, and pairs separated by a newline character.<br/>
        /// Null values are shown as 'NULL'
        /// </summary>
        public string ToLogFriendlyString()
        {
            return string.Join('\n', _params.Select(x => $"{x.Key}: {x.Value?.ToString() ?? "NULL"}"));
        }

        public override bool Equals(object obj)
        {
            if (obj is DynamicParams<T> dp)
            {
                return Equals(dp._params);
            }
            else if (obj is Dictionary<T, dynamic> @params)
            {
                return Equals(@params);
            }
            return false;
        }

        public bool Equals(Dictionary<T, dynamic> @params)
        {
            return _params.ContentEquals(@params);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            foreach (KeyValuePair<T, dynamic> param in _params)
            {
                hash = (hash * 7) + param.Key.GetHashCode();
                hash = (hash * 7) + param.Value.GetHashCode();
            }
            return hash;
        }

        private const string _expectedRegex = @"^;?(([^:;]+:[^:;]+);*)+$";

        /// <summary>
        /// Returns whether the input matches the expected regex of ^;?(([^:;]+:[^:;]+);*)+$<br/>
        /// <paramref name="keyValueSeparator"/> and <paramref name="pairSeparator"/> are ':' and ';' by default, but can be overridden.<br/>
        /// Null or whitespace strings return true.
        /// </summary>
        public static bool IsValidInput(string input, char keyValueSeparator = ':', char pairSeparator = ';')
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return true;
            }

            string paramPattern = $"^{pairSeparator}?(([^{keyValueSeparator}{pairSeparator}]+{keyValueSeparator}[^{keyValueSeparator}{pairSeparator}]+){pairSeparator}*)+$";
            Regex paramRegex = new Regex(paramPattern);
            return paramRegex.IsMatch(input);
        }

        /// <summary>
        /// Parse <paramref name="input"/> into <see cref="DynamicParams{T}"/> using the expected regex of ^;?(([^:;]+:[^:;]+);*)+$<br/>
        /// <paramref name="keyValueSeparator"/> and <paramref name="pairSeparator"/> are ':' and ';' by default, but can be overridden.<br/>
        /// Null or whitespace strings return an empty <see cref="DynamicParams{T}"/>.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="keyValueSeparator"/> and <paramref name="pairSeparator"/> are the same.<br/>
        /// Throws <see cref="ArgumentException"/> if invalid input (using <see cref="IsValidInput(string, char, char)"/>).<br/>
        /// Throws <see cref="ArgumentException"/> if unknown enum keys.<br/>
        /// Throws <see cref="AggregateException"/> of <see cref="ArgumentException"/>(s) if parsing value errors are thrown
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="AggregateException"></exception>
        public static DynamicParams<T> Parse(string input, DynamicParser parser, char keyValueSeparator = ':', char pairSeparator = ';')
        {
            if (keyValueSeparator == pairSeparator)
            {
                throw new ArgumentException($"DynamicParams: KeyValue and Pair separators cannot be the same");
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return new DynamicParams<T>();
            }

            if (!IsValidInput(input, keyValueSeparator, pairSeparator))
            {
                throw new ArgumentException($"DynamicParams: Input does not match regex ({_expectedRegex})");
            }

            // Split string into component parts
            Dictionary<string, string> stringParams =
                input.Split(pairSeparator, StringSplitOptions.RemoveEmptyEntries)
                     .Select(x => x.Split(keyValueSeparator))
                     .ToDictionary(x => x[0], x => x[1]);

            // Check for any unknown/invalid enum keys
            IEnumerable<string> unknownKeys = stringParams.Keys.Where(x => !EnumHelpers.IsDefined<T>(x, ignoreCase: true));
            if (unknownKeys.Any())
            {
                throw new ArgumentException($"Unknown {nameof(T)} keys: {string.Join(", ", unknownKeys)}");
            }

            // Parse keys into enum values
            Dictionary<T, string> enumParams = stringParams.ToDictionary(x => (T)Enum.Parse(typeof(T), x.Key), x => x.Value);

            DynamicParams <T> result = Parse(enumParams, parser, keyValueSeparator, pairSeparator);
            return result;
        }

        /// <summary>
        /// Parse <paramref name="input"/> into <see cref="DynamicParams{T}"/> using the expected regex of ^;?(([^:;]+:[^:;]+);*)+$<br/>
        /// <paramref name="keyValueSeparator"/> and <paramref name="pairSeparator"/> are ':' and ';' by default, but can be overridden.<br/>
        /// Null or whitespace strings return and empty <see cref="DynamicParams{T}"/>.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="keyValueSeparator"/> and <paramref name="pairSeparator"/> are the same.<br/>
        /// Throws <see cref="AggregateException"/> of <see cref="ArgumentException"/>(s) if parsing value errors are thrown
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="AggregateException"></exception>
        public static DynamicParams<T> Parse(Dictionary<T, string> @params, DynamicParser parser, char keyValueSeparator = ':', char pairSeparator = ';')
        {
            if (keyValueSeparator == pairSeparator)
            {
                throw new ArgumentException($"DynamicParams: KeyValue and Pair separators cannot be the same");
            }

            Dictionary<T, dynamic> result = ParseDictionaryToDynamic(@params, parser);
            return new DynamicParams<T>(result, keyValueSeparator, pairSeparator);
        }

        internal static Dictionary<T, dynamic> ParseDictionaryToDynamic(Dictionary<T, string> @params, DynamicParser parser)
        {
            ArgumentNullException.ThrowIfNull(@params);

            // Parse string values into dynamics
            Dictionary<T, dynamic> result = new Dictionary<T, dynamic>();
            List<string> errors = new List<string>();
            string enumName = typeof(T).Name;
            foreach (KeyValuePair<T, string> set in @params)
            {
                if (parser.CanCastToType(set.Key, set.Value))
                {
                    dynamic output = parser.CastToType(set.Key, set.Value);
                    result[set.Key] = output;
                }
                else
                {
                    errors.Add($"{set.Key.ToLongString()} cannot cast '{set.Value}' to {parser.ValueTypeName(set.Key)}");
                }
            }

            if (errors.Any())
            {
                throw new AggregateException($"Errors parsing input to {nameof(DynamicParams<T>)}", errors.Select(x => new ArgumentException(x)));
            }

            return result;
        }

        public abstract class DynamicParser
        {
            /// <summary>
            /// The name of the type the value of <paramref name="param"/> can be parsed into.<br/>
            /// Throws <see cref="NotImplementedException"/> if the <paramref name="param"/> has no expected type defined
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public abstract string ValueTypeName(T param);

            /// <summary>
            /// Returns whether <paramref name="input"/> can be parsed into the expected type, which can be found by <see cref="ValueTypeName(T)"/>.<br/>
            /// Throws <see cref="NotImplementedException"/> if the <paramref name="param"/> has no parsing check defined
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public abstract bool CanCastToType(T param, string input);

            /// <summary>
            /// Returns <paramref name="input"/> parsed into the expected type, which can be found by <see cref="ValueTypeName(T)"/>.<br/>
            /// Parsing exceptions can be thrown.<br/>
            /// Throws <see cref="NotImplementedException"/> if the <paramref name="param"/> has no parsing defined
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public abstract dynamic CastToType(T param, string input);

            protected readonly Dictionary<T, Func<dynamic, string>> _toStringOverrides = new Dictionary<T, Func<dynamic, string>>();

            /// <summary>
            /// Returns the ToString override for a given <paramref name="param"/>. Defaults to <see cref="ToString(T)"/> if no override found
            /// </summary>
            public virtual Func<dynamic, string> ToString(T param)
            {
                return _toStringOverrides.TryGetValue(param, out Func<dynamic, string> func) ? func : x => x.ToString();
            }

            protected bool CanDeserialize<S>(string input, params JsonConverter[] converters)
            {
                try
                {
                    JsonConvert.DeserializeObject<S>(input, converters);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            protected S Deserialize<S>(string input, params JsonConverter[] converters)
            {
                return JsonConvert.DeserializeObject<S>(input, converters);
            }
        }
    }
}
