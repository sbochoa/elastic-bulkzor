using System.Collections.Generic;
using Bulkzor.Configuration;

namespace Bulkzor.Commands
{
    public class BulkDocuments<TDocument>
        where TDocument : class 
    {
        public IEnumerable<TDocument> Documents { get; }
        public string IndexName { get; }
        public string TypeName { get; }
        public ChunkConfiguration ChunkConfiguration { get; }

        public BulkDocuments(IEnumerable<TDocument> documents, string indexName, string typeName, ChunkConfiguration chunkConfiguration)
        {
            Documents = documents;
            IndexName = indexName;
            TypeName = typeName;
            ChunkConfiguration = chunkConfiguration;
        }
    }
}
