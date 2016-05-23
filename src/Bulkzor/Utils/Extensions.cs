using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Utils
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> items,
                                                   int numOfParts)
        {
            int i = 0;
            return items.GroupBy(x => i++ % numOfParts);
        }

        public static string LogWithIndexDescription(this ILog log, string indexName, string typeName, string description)
        {
            return $"[index:{indexName}][type:{typeName}] {description}";
        }

        public static string GetIdFromUnknowObject(this object unknowObject)
        {
            return unknowObject.GetType().GetProperty("id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic).GetValue(unknowObject).ToString();
        }

        public static string GetValueFromObject(this object unknowObject, string name)
        {
            return unknowObject.GetType().GetProperty(name.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic).GetValue(unknowObject).ToString();
        }

        public static T GetConfigurationValue<T>(this JObject jsonObject, string name, T defaultValue = default(T), bool required = false)
        {
            JToken value;
            var hasValue = jsonObject.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out value);
            if (!hasValue)
                if (required)
                    throw new ArgumentException($"Parameter {name} is required");
                else
                    return defaultValue;
            return value.Value<T>();
        }
    }
}
