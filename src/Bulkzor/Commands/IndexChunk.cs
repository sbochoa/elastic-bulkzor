using System.Collections.Generic;
using Bulkzor.Configuration;

namespace Bulkzor.Commands
{
    public class IndexChunk<TDocument>
        where TDocument:class 
    {
        public IReadOnlyList<TDocument> Chunk { get; }
        public string IndexName { get; }
        public string TypeName { get; }
        public ChunkConfiguration ChunkConfiguration { get;  }

        public IndexChunk(IReadOnlyList<TDocument> chunk, string indexName, string typeName, ChunkConfiguration chunkConfiguration)
        {
            Chunk = chunk;
            IndexName = indexName;
            TypeName = typeName;
            ChunkConfiguration = chunkConfiguration;
        }
    }
}
