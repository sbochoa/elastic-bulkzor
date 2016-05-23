using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IIndexObjects
    {
        IndexObjectsResult Index(IReadOnlyList<object> objects, string indexName, string typeName);
    }
}
