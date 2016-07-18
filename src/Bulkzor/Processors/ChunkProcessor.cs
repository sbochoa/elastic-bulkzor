using System;
using Bulkzor.Configuration;
using Bulkzor.Errors;
using Bulkzor.Indexers;
using Bulkzor.Models;
using Bulkzor.Results;
using Bulkzor.Utilities;
using Common.Logging;

namespace Bulkzor.Processors
{
    public class ChunkProcessor
    {
        private readonly ChunkConfiguration _chunkConfiguration;
        private readonly IObjectIndexer _objectIndexer;
        private readonly IndexErrorHandler _indexErrorsHandler;
        private readonly ILog _logger;

        public ChunkProcessor(ChunkConfiguration chunkConfiguration,
                             IObjectIndexer objectIndexer, 
                             IndexErrorHandler indexErrorsHandler, 
                             ILog logger)
        {
            _chunkConfiguration = chunkConfiguration;
            _objectIndexer = objectIndexer;
            _indexErrorsHandler = indexErrorsHandler;
            _logger = logger;
        }

        public ObjectsProcessedResult ProcessChunk(Chunk chunk)
        {
            Func<string, string> logWithIndexDescription =
                description => _logger.LogWithIndexDescription(chunk.IndexName, chunk.TypeName, description);

            _logger.Info(logWithIndexDescription("Started to index chunk"));

            var indexDocumentsResult = _objectIndexer.Index(chunk, chunk.IndexName, chunk.TypeName);

            if (indexDocumentsResult.HaveError && _indexErrorsHandler != null)
            {
                indexDocumentsResult 
                    = _indexErrorsHandler.HandleError(indexDocumentsResult.Error, indexDocumentsResult.ObjectsNotIndexed, chunk.IndexName, chunk.TypeName);
            }
            else
            {
                _logger.Info(logWithIndexDescription($"Ended index chunk - Indexed:{indexDocumentsResult.ObjectsIndexed} Not Indexed:{indexDocumentsResult.ObjectsNotIndexed}"));
            }

            return new ObjectsProcessedResult(indexDocumentsResult.ObjectsIndexed.Count, indexDocumentsResult.ObjectsNotIndexed.Count, indexDocumentsResult.ObjectsNotIndexedStored.Count);
        }

        public bool IsChunkFull(Chunk chunk)
        {
            return chunk.Count >= _chunkConfiguration.ChunkSize;
        }
    }
}
