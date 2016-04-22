using System.Collections.Generic;
using Bulkzor.Utils;

namespace Bulkzor.Indexers
{
    public class DataIndexChunk<T>
    {
        public DataIndexChunk(string indexName, string typeName)
        {
            IndexName = indexName;
            TypeName = typeName;
            DataIndex = new List<T>();
        }

        public string IndexName { get; }
        public string TypeName { get; }
        public ICollection<T> DataIndex { get; }

        public IEnumerable<IEnumerable<T>> GetDataIndexInParts(int partsQuantity)
        {
            return DataIndex.Split(partsQuantity);
        }
    }
}