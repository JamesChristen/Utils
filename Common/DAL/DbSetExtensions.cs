using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Common.DAL
{
    internal static class DbSetExtensions
    {
        internal static string Include<T1, T2>(Expression<Func<T1, T2>> propertySelector)
        {
            // Expected format: x.ABC[.DEF][.GHI][...] => required: ABC[.DEF][.GHI][...]
            string body = Regex.Match(propertySelector.Body.ToString(), @"\.(.*)").Value.Trim('.');
            return body;
        }

        internal static T GetValueOrDefault<T>(this IDataRecord row, string fieldName)
        {
            int ordinal = row.GetOrdinal(fieldName);
            return row.GetValueOrDefault<T>(ordinal);
        }

        internal static T GetValueOrDefault<T>(this IDataRecord row, int ordinal)
        {
            return (T)(row.IsDBNull(ordinal) ? default(T) : row.GetValue(ordinal));
        }
    }
}
