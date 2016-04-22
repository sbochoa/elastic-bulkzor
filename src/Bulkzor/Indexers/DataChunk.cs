using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Utils;

namespace Bulkzor.Indexers
{
    public class DataChunk<T>
    {
        private readonly Func<T, string> _indexNameFunc;

        private readonly int _maximumSize;
        private readonly string _typeName;
        private readonly string _indexName;

        public bool IsFull => _maximumSize <= Data.Sum(ic => ic.DataIndex.Count);
        public bool HasData => Data.Any();

        private DataChunk(string typeName, int maximumSize)
        {
            _typeName = typeName;
            _maximumSize = maximumSize;

            Data = new List<DataIndexChunk<T>>();
        }

        public DataChunk(Func<T, string> indexNameFunc, string typeName,  int maximumSize)
            : this(typeName, maximumSize)
        {
            _indexNameFunc = indexNameFunc;
            _typeName = typeName;
        }

        public DataChunk(string indexName, string typeName, int maximumSize)
            : this(typeName, maximumSize)
        {
            _indexName = indexName;
        }

        public ICollection<DataIndexChunk<T>> Data { get; }

        public void Add(T @object)
        {
            var indexName = _indexName ?? _indexNameFunc(@object);
            var typeName = _typeName;

            var dataIndex = Data.FirstOrDefault(d => d.IndexName == indexName);

            if (dataIndex == null)
            {
                dataIndex = new DataIndexChunk<T>(indexName, typeName);
                Data.Add(dataIndex);
            }

            dataIndex.DataIndex.Add(@object);
        }

        public void ClearData()
        {
            Data.Clear();
        }

        //public IEnumerable<IEnumerable<DataIndexChunk<T>>> GetDataInParts(int partsQuantity)
        //{
        //    return Data.Split(partsQuantity);
        //}
    }
}