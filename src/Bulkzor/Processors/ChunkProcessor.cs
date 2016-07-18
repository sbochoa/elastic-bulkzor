using System;
using Bulkzor.Configuration;
using Bulkzor.Indexers;
using Bulkzor.Models;
using Bulkzor.Results;
using Bulkzor.Storage;
using Bulkzor.Utilities;
using Common.Logging;

namespace Bulkzor.Processors
{
    public class ChunkProcessor
    {
        private readonly ChunkConfiguration _chunkConfiguration;
        private readonly IObjectIndexer _objectIndexer;
        private readonly IStoreObjects _objectsStore;
        private readonly ILog _logger;
        private const string ObjectsNotIndexedFolder = "NotIndexed";

        public ChunkProcessor(ChunkConfiguration chunkConfiguration,
                             IObjectIndexer objectIndexer,
                             IStoreObjects objectsStore,
                             ILog logger)
        {
            _chunkConfiguration = chunkConfiguration;
            _objectIndexer = objectIndexer;
            _objectsStore = objectsStore;
            _logger = logger;
        }

        public ObjectsProcessedResult ProcessChunk(Chunk chunk)
        {
            Func<string, string> logWithIndexDescription =
                description => _logger.LogWithIndexDescription(chunk.IndexName, chunk.TypeName, description);

            _logger.Info(logWithIndexDescription("Started to index chunk"));

            var indexDocumentsResult = _objectIndexer.Index(chunk, chunk.IndexName, chunk.TypeName);

            if (indexDocumentsResult.HaveError)
            {
                _objectsStore.StoreObjects(ObjectsNotIndexedFolder, indexDocumentsResult.ObjectsNotIndexed, chunk.IndexName, chunk.TypeName);

                _logger.Warn(logWithIndexDescription($"Error of type { indexDocumentsResult.Error.GetType()} ocurred indexing the chunk"));
            }

            _logger.Info(logWithIndexDescription($"Ended index chunk - Indexed:{indexDocumentsResult.ObjectsIndexed} Not Indexed:{indexDocumentsResult.ObjectsNotIndexed}"));

            return new ObjectsProcessedResult(indexDocumentsResult.ObjectsIndexed.Count, indexDocumentsResult.ObjectsNotIndexed.Count, indexDocumentsResult.ObjectsNotIndexedStored.Count);
        }

        public bool IsChunkFull(Chunk chunk)
        {
            return chunk.Count >= _chunkConfiguration.ChunkSize;
        }
    }
}
