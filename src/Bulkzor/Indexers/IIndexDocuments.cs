using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IIndexDocuments
    {
        IndexDocumentsResult IndexDocuments<T>(IEnumerable<T> documents, string indexName, string typeName) where T : class;
    }
}
