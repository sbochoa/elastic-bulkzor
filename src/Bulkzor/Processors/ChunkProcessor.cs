using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Indexers;
using Bulkzor.Models;
using Bulkzor.Results;
using Bulkzor.Storage;
using Bulkzor.Types;
using Bulkzor.Utils;
using Common.Logging;
using static System.String;

namespace Bulkzor.Processors
{
    public class ChunkProcessor
        : IProcessChunks
    {
        private readonly IIndexObjects _indexObjects;
        private readonly IStoreObjects _objectsStore;
        private readonly ILog _logger;
        private const int PartsQuantity = 3;

        public ChunkProcessor(IIndexObjects indexObjects, IStoreObjects objectsStore, ILog logger)
        {
            _indexObjects = indexObjects;
            _objectsStore = objectsStore;
            _logger = logger;
        }

        public ObjectsProcessedResult ProcessChunks<T>(IReadOnlyList<Chunk<T>> chunks)
            where T: class, IIndexableObject
        {
            var objectsIndexed = 0;
            var objectsNotIndexed = 0;

            foreach (var chunk in chunks)
            {
                Func<string, string> logWithIndexDescription =
                    description => _logger.LogWithIndexDescription(chunk.IndexName, chunk.TypeName, description);

                _logger.Info(logWithIndexDescription("Started to index chunk"));

                var indexDocumentsResult = _indexObjects.Index(chunk.Data, chunk.IndexName, chunk.TypeName);

                if (indexDocumentsResult.HaveError)
                {
                    switch (indexDocumentsResult.Error)
                    {
                        case IndexingError.LengthExceeded:
                            _logger.Warn(logWithIndexDescription($"Retrying partings chunk in {PartsQuantity} parts"));
                            indexDocumentsResult = IndexDataChunkInParts(indexDocumentsResult.ObjectsNotIndexed, chunk.IndexName, chunk.TypeName, PartsQuantity);
                            break;
                        case IndexingError.OnlyPartOfDocumentsIndexed:
                        case IndexingError.Unknow:
                            _logger.Warn(logWithIndexDescription($"Storing data not indexed"));
                             _objectsStore.StoreObjects(indexDocumentsResult.ObjectsNotIndexed, chunk.IndexName, chunk.TypeName);
                            break;
                        default: throw new ArgumentException(nameof(indexDocumentsResult.Error));
                    }
                }
                else
                {
                    _logger.Info(logWithIndexDescription($"Ended index chunk - Indexed:{indexDocumentsResult.ObjectsIndexed} Not Indexed:{indexDocumentsResult.ObjectsNotIndexed}"));
                }

                objectsIndexed += indexDocumentsResult.ObjectsIndexed.Count;
                objectsNotIndexed += indexDocumentsResult.ObjectsNotIndexed.Count;
            }

            return new ObjectsProcessedResult(objectsIndexed, objectsNotIndexed);
        }

        private IndexObjectsResult<T> IndexDataChunkInParts<T>(IReadOnlyList<T> objectsWithErrors, string indexName, string typeName, int partsQuantity)
            where T : class, IIndexableObject
        {
            var chunkDataParts = objectsWithErrors.Split(partsQuantity).ToList();
            var i = 0;

            Func<string, string> logWithIndexDescription =
                    description => _logger.LogWithIndexDescription(indexName, typeName, description);

            _logger.Warn(logWithIndexDescription($"Parted chunk in {partsQuantity} parts: { Join("-", chunkDataParts.Select(cp => $"Part {++i}:{cp.Count()}"))}".TrimEnd('-')));

            var documentsIndexed = new List<T>();
            var documentsNotIndexed = new List<T>();

            i = 0;

            foreach (var chunkData in chunkDataParts)
            {
                var indexDocumentsResult = _indexObjects.Index(chunkData.ToList(), indexName, typeName);

                if (indexDocumentsResult.HaveError)
                {
                    _objectsStore.StoreObjects(objectsWithErrors, indexName, typeName);
                    documentsNotIndexed.AddRange(indexDocumentsResult.ObjectsNotIndexed);
                }

                documentsIndexed.AddRange(indexDocumentsResult.ObjectsIndexed);

                _logger.Warn(logWithIndexDescription($"Part {++i}: Indexed:{indexDocumentsResult.ObjectsIndexed} - Not Indexed:{indexDocumentsResult.ObjectsNotIndexed}"));
            }

            return new IndexObjectsResult<T>(documentsIndexed, documentsNotIndexed);
        }
    }
}
