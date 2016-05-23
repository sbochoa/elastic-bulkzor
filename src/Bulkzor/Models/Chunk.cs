using System.Collections.Generic;
using Bulkzor.Utils;

namespace Bulkzor.Models
{
    public class Chunk
    {
        public Chunk(string indexName, string typeName)
        {
            IndexName = indexName;
            TypeName = typeName;
            _data = new List<object>();
        }

        public string IndexName { get; }
        public string TypeName { get; }
        private readonly List<object> _data;

        public IReadOnlyList<object> Data => _data; 

        public void AddObject(object @object)
        {
            _data.Add(@object);
        }

        public bool HasIndexName(string indexName)
        {
            return IndexName == indexName;
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}