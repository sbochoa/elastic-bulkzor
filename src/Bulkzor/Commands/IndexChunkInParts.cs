using System.Collections.Generic;

namespace Bulkzor.Commands
{
    public class IndexChunkInParts<TDocument>
        where TDocument : class 
    {
        public IReadOnlyList<TDocument> Chunk { get; }
        public string IndexName { get; }
        public string TypeName { get; }
        public int NumberParts { get; }

        public IndexChunkInParts(IReadOnlyList<TDocument> chunk, string indexName, string typeName, int numberParts)
        {
            Chunk = chunk;
            IndexName = indexName;
            TypeName = typeName;
            NumberParts = numberParts;
        }
    }
}
