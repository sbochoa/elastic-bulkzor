using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bulkzor.Models;
using Bulkzor.Results;
using Common.Logging;

namespace Bulkzor.Processors
{
    public class DataProcessor
    {
        private readonly ChunkProcessor _chunkProcessor;
        private readonly ILog _logger;
        private readonly IList<Chunk> _chunks = new List<Chunk>(); 

        public DataProcessor(ChunkProcessor chunkProcessor, ILog logger)
        {
            _chunkProcessor = chunkProcessor;
            _logger = logger;
        }

        private Chunk GetChunkByIndexName(string indexName, string typeName)
        {
            var chunk = _chunks.FirstOrDefault(x => x.HasIndexName(indexName));
            if (chunk != null)
            {
                return chunk;
            }

            chunk = new Chunk(indexName, typeName);
            _chunks.Add(chunk);
            return chunk;
        }

        public ObjectsProcessedResult IndexData(IEnumerable<object> data, Func<object, string> indexNameBuilder, string typeName)
        {
            var objectCount = 0;
            var result = new ObjectsProcessedResult();

            var watch = new Stopwatch();
            watch.Start();

            _logger.Info($"Started to index data");

            foreach (var @object in data)
            {
                var indexName = indexNameBuilder(@object);
                var chunk = GetChunkByIndexName(indexName, typeName);
                chunk.Add(@object);
                var processChunkResult = ProcessChunk(chunk);
                result.Add(processChunkResult);

                ++objectCount;
            }

            ProcessPendingChunks(result);
            watch.Stop();

            _logger.Info($"Index data ended - Total: {objectCount} Indexed: {result.ObjectsProcessed} Not Indexed: {result.ObjectsNotProcessed} Took: {watch.Elapsed.ToString("g")}");

            return result;
        }

        private void ProcessPendingChunks(ObjectsProcessedResult result)
        {
            foreach (var chunk in _chunks)
            {
                var processChunkResult = _chunkProcessor.ProcessChunk(chunk);
                result.Add(processChunkResult);
            }

            _chunks.Clear();
        }

        private ObjectsProcessedResult ProcessChunk(Chunk chunk)
        {
            if (_chunkProcessor.IsChunkFull(chunk))
            {
                var result = _chunkProcessor.ProcessChunk(chunk);
                _chunks.Remove(chunk);
                return result;
            }

            return ObjectsProcessedResult.Default;
        }
    }
}
