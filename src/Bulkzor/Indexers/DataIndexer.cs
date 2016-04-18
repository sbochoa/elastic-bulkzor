using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bulkzor.Configuration;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public class DataIndexer
        : IIndexData
    {
        private readonly IIndexDataChunk _dataChunkIndexer;

        public DataIndexer(IIndexDataChunk dataChunkIndexer)
        {
            _dataChunkIndexer = dataChunkIndexer;
        }

        public IndexResult IndexData<T>(IEnumerable<T> data, string indexName, string typeName, ChunkConfiguration chunkConfiguration)
            where T : class
        {
            var dataChunk = new DataChunk<T>(indexName, typeName, chunkConfiguration.GetChunkSize);
            var watch = new Stopwatch();

            var objectsIndexed = 0;
            var objectsNotIndexed = 0;

            Action indexDataChunk = () =>
            {
                var result = _dataChunkIndexer.IndexDataChunk(dataChunk);
                objectsIndexed += result.ObjectsIndexed;
                objectsNotIndexed += result.ObjectsNotIndexed;
                chunkConfiguration.GetOnChunkIndexed?.Invoke(result, indexName, typeName);
            };

            watch.Start();

            foreach (var @object in data)
            {
                if (dataChunk.IsFull)
                {
                    indexDataChunk();
                    dataChunk.ClearData();
                }

                dataChunk.Add(@object);
            }

            indexDataChunk();

            watch.Stop();
            
            return new IndexResult(objectsIndexed, objectsNotIndexed, watch.Elapsed);
        }
    }
}
