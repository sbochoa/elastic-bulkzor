using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IIndexObjects
    {
        IndexObjectsResult<T> Index<T>(IReadOnlyList<T> objects, string indexName, string typeName)
            where T : class, IIndexableObject;
    }
}
