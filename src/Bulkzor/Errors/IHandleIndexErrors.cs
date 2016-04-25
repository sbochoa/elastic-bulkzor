using System.Collections.Generic;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Errors
{
    public interface IHandleIndexErrors<T>
        where T : class, IIndexableObject
    {
        IndexObjectsResult<T> HandleError(IndexingError error, IReadOnlyList<T> objectsNotIndexed, string indexName, string typeName);
    }
}
