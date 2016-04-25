using System;
using System.Collections.Generic;
using System.Linq;

namespace Bulkzor.Models
{
    public class ChunkStore<T>
    {
        private readonly Func<T, string> _indexNameFunc;

        private readonly int _maximumSize;
        private readonly string _typeName;
        private readonly string _indexName;
        private readonly List<Chunk<T>> _chunks;

        public bool IsFull => _maximumSize <= _chunks.Sum(ic => ic.Data.Count);
        public bool HasData => _chunks.Any();

        public IReadOnlyList<Chunk<T>> Chunks => _chunks;

        private ChunkStore(string typeName, int maximumSize)
        {
            _typeName = typeName;
            _maximumSize = maximumSize;
            _chunks = new List<Chunk<T>>();
        }

        public ChunkStore(Func<T, string> indexNameFunc, string typeName,  int maximumSize)
            : this(typeName, maximumSize)
        {
            _indexNameFunc = indexNameFunc;
            _typeName = typeName;
        }

        public ChunkStore(string indexName, string typeName, int maximumSize)
            : this(typeName, maximumSize)
        {
            _indexName = indexName;
        }

        public void AddObjectToChunk(T @object)
        {
            if (IsFull)
            {
                throw new ApplicationException("Cant add more objects Chunk is full");
            }

            var indexName = _indexName ?? _indexNameFunc(@object);
            var typeName = _typeName;

            var chunk = _chunks.FirstOrDefault(d => d.HasIndexName(indexName));

            if (chunk == null)
            {
                chunk = new Chunk<T>(indexName, typeName);
                _chunks.Add(chunk);
            }

            chunk.AddObject(@object);
        }

        public void ClearChunks()
        {
            _chunks.Clear();
        }
    }
}