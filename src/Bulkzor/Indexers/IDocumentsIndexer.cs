using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IDocumentsIndexer
    {
        IndexResult Index<T>(IEnumerable<T> documents, string indexName, string typeName)
            where T : class;
    }
}
