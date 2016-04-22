using System.Collections.Generic;
using System.Linq;
using Bulkzor.Utils;

namespace Bulkzor.Indexers
{
    public class DataChunk<T>
    {
        private readonly int _maximumSize;
        public string IndexName { get; }
        public string TypeName { get; }

        public bool IsFull => _maximumSize <= Data.Count;
        public bool HasData => Data.Any();

        public DataChunk(string indexName, string typeName, int maximumSize)
        {
            _maximumSize = maximumSize;
            IndexName = indexName;
            TypeName = typeName;
            Data = new List<T>();
        }

        public ICollection<T> Data { get; }

        public void Add(T @object)
        {
            Data.Add(@object);
        }

        public void ClearData()
        {
            Data.Clear();
        }

        public IEnumerable<IEnumerable<T>> GetDataInParts(int partsQuantity)
        {
            return Data.Split(partsQuantity);
        } 
    }
}