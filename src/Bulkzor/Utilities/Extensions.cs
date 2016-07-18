using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Utilities
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
            var value = unknowObject.GetType().GetProperty("id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic).GetValue(unknowObject);

            Check.NotNull(value, "Id Field");

            return value.ToString();
        }

        public static string GetValueFromObject(this object unknowObject, string name)
        {
            DebugCheck.NotNull(unknowObject);

            return unknowObject.GetType().GetProperty(name.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic).GetValue(unknowObject).ToString();
        }

        public static T GetConfigurationValue<T>(this JObject jsonObject, string name, T defaultValue)
        {
            DebugCheck.NotNull(jsonObject);

            JToken value;
            var hasValue = jsonObject.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out value);
            if (!hasValue)
                return defaultValue;
            return value.Value<T>();
        }
        public static T GetConfigurationValue<T>(this JObject jsonObject, string name)
        {
            DebugCheck.NotNull(jsonObject);

            JToken value;
            var hasValue = jsonObject.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out value);
            if (!hasValue)
                throw new ArgumentException($"Parameter {name} is required");
            return value.Value<T>();
        }
    }
}
