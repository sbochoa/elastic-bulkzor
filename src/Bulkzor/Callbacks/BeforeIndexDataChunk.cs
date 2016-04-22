using Bulkzor.Indexers;
using Bulkzor.Results;

namespace Bulkzor.Callbacks
{
    public delegate void BeforeIndexDataChunk(IIndexDocuments documentsIndexer, string indexName, string typeName);
}