using System;
using System.Collections.Generic;
using Bulkzor.Errors;
using Bulkzor.Indexers;
using Bulkzor.Models;
using Bulkzor.Results;
using Bulkzor.Utils;
using Common.Logging;

namespace Bulkzor.Processors
{
    public class ChunkProcessor : IProcessChunks
    {
        private readonly IIndexObjects _indexObjects;
        private readonly IHandleIndexErrors _indexErrorsHandler;
        private readonly ILog _logger;

        public ChunkProcessor(IIndexObjects indexObjects, IHandleIndexErrors indexErrorsHandler, ILog logger)
        {
            _indexObjects = indexObjects;
            _indexErrorsHandler = indexErrorsHandler;
            _logger = logger;
        }

        public ObjectsProcessedResult ProcessChunks(IReadOnlyList<Chunk> chunks)
        {
            var objectsIndexed = 0;
            var objectsNotIndexed = 0;
            var objectsNotIndexedStored = 0;

            foreach (var chunk in chunks)
            {
                Func<string, string> logWithIndexDescription =
                    description => _logger.LogWithIndexDescription(chunk.IndexName, chunk.TypeName, description);

                _logger.Info(logWithIndexDescription("Started to index chunk"));

                var indexDocumentsResult = _indexObjects.Index(chunk.Data, chunk.IndexName, chunk.TypeName);

                if (indexDocumentsResult.HaveError)
                {
                    indexDocumentsResult 
                        = _indexErrorsHandler.HandleError(indexDocumentsResult.Error, indexDocumentsResult.ObjectsNotIndexed, chunk.IndexName, chunk.TypeName);
                }
                else
                {
                    _logger.Info(logWithIndexDescription($"Ended index chunk - Indexed:{indexDocumentsResult.ObjectsIndexed} Not Indexed:{indexDocumentsResult.ObjectsNotIndexed}"));
                }

                objectsIndexed += indexDocumentsResult.ObjectsIndexed.Count;
                objectsNotIndexed += indexDocumentsResult.ObjectsNotIndexed.Count;
                objectsNotIndexedStored += indexDocumentsResult.ObjectsNotIndexedStored.Count;
            }

            return new ObjectsProcessedResult(objectsIndexed, objectsNotIndexed, objectsNotIndexedStored);
        }
    }
}
