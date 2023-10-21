using System;
using System.Linq;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        /// <summary>Joins all KeyValuePairs by the <paramref name="keyValueSeparator"/> 
        /// and join pairs by the <paramref name="pairSeparator"/>, into a single string</summary>
        public static string ToString<T, S>(this Dictionary<T, S> dict, char keyValueSeparator, char pairSeparator)
        {
            ArgumentNullException.ThrowIfNull(dict);

            return string.Join(pairSeparator, dict.Select(x => $"{x.Key}{keyValueSeparator}{x.Value}"));
        }

        /// <summary>Returns true if every KeyValuePair in source is present in <paramref name="other"/> and vice versa</summary>
        public static bool ContentEquals<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> other)
        {
            Dictionary<TKey, string> dict1 = dict?.ToDictionary(x => x.Key, x => x.Value?.ToString() ?? "NULL") ?? new Dictionary<TKey, string>();
            Dictionary<TKey, string> dict2 = other?.ToDictionary(x => x.Key, x => x.Value?.ToString() ?? "NULL") ?? new Dictionary<TKey, string>();

            if (dict1.Count != dict2.Count)
            {
                return false;
            }

            return dict1.OrderBy(kvp => kvp.Key)
                        .SequenceEqual(dict2.OrderBy(kvp => kvp.Key));
        }

        /// <summary>Returns true if <paramref name="other"/> is a subset of source. False if <paramref name="other"/> is a superset</summary>
        public static bool ContentContains<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> other)
        {
            Dictionary<TKey, string> dict1 = dict?.ToDictionary(x => x.Key, x => x.Value?.ToString() ?? "NULL") ?? new Dictionary<TKey, string>();
            Dictionary<TKey, string> dict2 = other?.ToDictionary(x => x.Key, x => x.Value?.ToString() ?? "NULL") ?? new Dictionary<TKey, string>();

            if (dict1.Count < dict2.Count)
            {
                return false;
            }

            foreach (KeyValuePair<TKey, string> set in dict2)
            {
                if (!dict1.TryGetValue(set.Key, out string val1) || !val1.Equals(set.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
