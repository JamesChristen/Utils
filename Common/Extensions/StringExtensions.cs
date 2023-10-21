using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes all WhiteSpace. Null input returns null
        /// </summary>
        public static string RemoveWhiteSpace(this string s)
        {
            if (s == null)
            {
                return null;
            }
            return new string(s.Where(x => !char.IsWhiteSpace(x)).ToArray());
        }

        /// <summary>
        /// Returns string with only a-z, A-Z, 0-9, and underscores. Null input returns null
        /// </summary>
        public static string SqlClean(this string s)
        {
            if (s == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
