using Bulkzor.Indexers;
using Bulkzor.Results;

namespace Bulkzor.Callbacks
{
    public delegate void AfterBulkTaskRun(IIndexDocuments documentsIndexer, string indexName, string typeName, IndexResult result);
}