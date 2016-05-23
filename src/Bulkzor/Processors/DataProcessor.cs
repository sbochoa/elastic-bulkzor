using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bulkzor.Models;
using Bulkzor.Results;
using Common.Logging;

namespace Bulkzor.Processors
{
    public class DataProcessor : IProcessData
    {
        private readonly IProcessChunks _chunksIndexer;
        private readonly ILog _logger;

        public DataProcessor(IProcessChunks chunksIndexer, ILog logger)
        {
            _chunksIndexer = chunksIndexer;
            _logger = logger;
        }

        public ObjectsProcessedResult IndexData(IEnumerable<object> data, Func<object, string> indexNameFunc, string typeName, int chunkSize)
        {
            var chunkStore = new ChunkStore(indexNameFunc, typeName, chunkSize);

            var objectsProcessed = 0;
            var objectsNotProcessed = 0;
            var objectsNotProcessedStored = 0;
            var objectCount = 0;

            Action indexDataChunk = () =>
            {
                var result = _chunksIndexer.ProcessChunks(chunkStore.Chunks);
                objectsProcessed += result.ObjectsProcessed;
                objectsNotProcessed += result.ObjectsNotProcessed;
                objectsNotProcessedStored += result.ObjectsNotProcessedStored;
            };

            var watch = new Stopwatch();

            watch.Start();

            _logger.Info($"Started to index data");

            foreach (var @object in data)
            {
                if (chunkStore.IsFull)
                {
                    indexDataChunk();
                    chunkStore.ClearChunks();
                }

                chunkStore.AddObjectToChunk(@object);
                objectCount++;
            }

            if (chunkStore.HasData)
            {
                indexDataChunk();
            }

            watch.Stop();

            _logger.Info($"Index data ended - Total:{objectCount} Indexed:{objectsProcessed} Not Indexed:{objectsNotProcessed} Took:{watch.Elapsed.ToString("g")}");

            return new ObjectsProcessedResult(objectsProcessed, objectsNotProcessed, objectsNotProcessedStored);
        }
    }
}
