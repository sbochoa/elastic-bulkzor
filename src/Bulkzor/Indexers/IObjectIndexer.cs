using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IObjectIndexer
    {
        IndexObjectsResult Index(IReadOnlyList<object> objects, string indexName, string typeName);
    }
}
