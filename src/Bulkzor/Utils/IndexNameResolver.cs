using System;
using System.Linq.Expressions;

namespace Bulkzor.Utils
{
    public class IndexNameResolver
    {
        public string ResolveIndexName<T>(Expression<Func<T, string>> indexNameExpression)
            where T : class
        {
            var indexNameFunc = indexNameExpression.Compile();

            return "true";
        }
    }
}
