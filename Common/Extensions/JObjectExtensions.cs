using System;

namespace Newtonsoft.Json.Linq
{
    public static class JObjectExtensions
    {
        public static JToken Get(this JObject jo, string field)
        {
            return jo.GetValue(field, StringComparison.OrdinalIgnoreCase);
        }

        public static JToken GetOrNull(this JObject jo, string field)
        {
            if (jo.ContainsKey(field))
            {
                return jo.Get(field);
            }
            return null;
        }
    }
}
