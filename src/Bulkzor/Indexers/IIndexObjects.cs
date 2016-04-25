using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IIndexObjects<T>
        where T : class, IIndexableObject
    {
        IndexObjectsResult<T> Index(IReadOnlyList<T> objects, string indexName, string typeName);
    }
}
