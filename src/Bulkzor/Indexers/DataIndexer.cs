using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var dataChunkWatch = new Stopwatch();

            var objectsIndexed = 0;
            var objectsNotIndexed = 0;

            Action indexDataChunk = () =>
            {
                var result = _dataChunkIndexer.IndexDataChunk(dataChunk);
                objectsIndexed += result.ObjectsIndexed;
                objectsNotIndexed += result.ObjectsNotIndexed;
                dataChunkWatch.Stop();
                chunkConfiguration.GetOnDataChunkIndexed?.Invoke(result, indexName, typeName);
            };

            watch.Start();
            dataChunkWatch.Start();

            foreach (var @object in data)
            {
                if (dataChunk.IsFull)
                {
                    indexDataChunk();
                    dataChunk.ClearData();

                    dataChunkWatch.Restart();
                }

                dataChunk.Add(@object);
            }

            if (dataChunk.HasData)
            {
                indexDataChunk();
            }

            watch.Stop();
            
            return new IndexResult(objectsIndexed, objectsNotIndexed, watch.Elapsed);
        }
    }
}
