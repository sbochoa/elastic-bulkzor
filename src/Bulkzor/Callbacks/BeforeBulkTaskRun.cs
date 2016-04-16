using Bulkzor.Indexers;

namespace Bulkzor.Callbacks
{
    public delegate void BeforeBulkTaskRun(IDocumentsIndexer documentsIndexer, string indexName, string typeName);
}