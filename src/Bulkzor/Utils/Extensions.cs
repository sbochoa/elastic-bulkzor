using System.Collections.Generic;
using System.Linq;

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
    }
}
