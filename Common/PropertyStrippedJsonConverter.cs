using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json
{
    public abstract class PropertyStrippedJsonConverter : JsonConverter
    {
        public List<string> RemovableProperties = new List<string> { "ValidationIdentifier" };
        public List<Action<JObject, object>> AdditionalPropertySetters = new List<Action<JObject, object>>();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JsonSerializer s = new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            serializer.Converters.Where(x => x != this).ForEach(x => s.Converters.Add(x));

            JToken t = JToken.FromObject(value, s);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                foreach (string property in RemovableProperties)
                {
                    o.Remove(property);
                }

                foreach (Action<JObject, object> additionalPropertySetter in AdditionalPropertySetters)
                {
                    additionalPropertySetter(o, value);
                }

                o.WriteTo(writer);
            }
        }
    }
}
