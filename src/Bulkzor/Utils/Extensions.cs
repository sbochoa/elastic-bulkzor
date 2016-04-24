using System.Collections.Generic;
using System.Linq;
using Common.Logging;

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
    }
}
