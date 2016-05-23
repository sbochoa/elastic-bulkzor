using System.Collections.Generic;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Errors
{
    public interface IHandleIndexErrors
    {
        IndexObjectsResult HandleError(IndexingError error, IReadOnlyList<object> objectsNotIndexed, string indexName, string typeName);
    }
}
