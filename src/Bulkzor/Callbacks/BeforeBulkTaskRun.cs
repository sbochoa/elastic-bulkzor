using Bulkzor.Indexers;

namespace Bulkzor.Callbacks
{
    public delegate void BeforeBulkTaskRun(IIndexDocuments documentsIndexer, string indexName, string typeName);
}