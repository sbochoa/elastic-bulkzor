using System.Collections.Generic;
using Bulkzor.Utils;

namespace Bulkzor.Models
{
    public class Chunk<T>
    {
        public Chunk(string indexName, string typeName)
        {
            IndexName = indexName;
            TypeName = typeName;
            _data = new List<T>();
        }

        public string IndexName { get; }
        public string TypeName { get; }
        private readonly List<T> _data;

        public IReadOnlyList<T> Data => _data; 

        public void AddObject(T @object)
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