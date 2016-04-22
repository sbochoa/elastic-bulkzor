using Bulkzor.Indexers;
using Bulkzor.Results;

namespace Bulkzor.Callbacks
{
    public delegate void AfterIndexDataChunk(IIndexDocuments documentsIndexor, IndexDataChunkResult result, string indexName, string typeName);
}