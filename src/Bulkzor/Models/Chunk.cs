using System.Collections.Generic;

namespace Bulkzor.Models
{
    public class Chunk : List<object>
    {
        public Chunk(string indexName, string typeName)
        {
            IndexName = indexName;
            TypeName = typeName;
        }

        public string IndexName { get; }
        public string TypeName { get; }

        public bool HasIndexName(string indexName)
        {
            return IndexName == indexName;
        }
    }
}