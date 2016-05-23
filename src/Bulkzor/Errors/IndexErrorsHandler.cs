﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Bulkzor.Storage;
using Bulkzor.Types;
using Bulkzor.Utils;
using Common.Logging;
using static System.String;

namespace Bulkzor.Errors
{
    public class IndexErrorsHandler : IHandleIndexErrors
    {
        private readonly IIndexObjects _objectsIndexer;
        private readonly IStoreObjects _objectsStore;
        private readonly ILog _logger;
        private const int PartsQuantity = 3;

        public IndexErrorsHandler(IIndexObjects objectsIndexer, IStoreObjects objectsStore, ILog logger)
        {
            _objectsIndexer = objectsIndexer;
            _objectsStore = objectsStore;
            _logger = logger;
        }

        public IndexObjectsResult HandleError(IndexingError error, IReadOnlyList<object> objectsNotIndexed, string indexName, string typeName) 
        {
            Func<string, string> logWithIndexDescription =
                    description => _logger.LogWithIndexDescription(indexName, typeName, description);

            switch (error)
            {
                case IndexingError.LengthExceeded:
                    _logger.Warn(logWithIndexDescription($"Retrying partings chunk in {PartsQuantity} parts"));
                    return IndexDataChunkInParts(objectsNotIndexed, indexName, typeName, PartsQuantity);
                case IndexingError.OnlyPartOfDocumentsIndexed:
                case IndexingError.Unknow:
                    _logger.Warn(logWithIndexDescription($"Storing data not indexed"));
                    _objectsStore.StoreObjects(objectsNotIndexed, indexName, typeName);
                    return new IndexObjectsResult(new List<object>(), objectsNotIndexed, IndexingError.OnlyPartOfDocumentsIndexed, objectsNotIndexed);
                default:
                    throw new ArgumentException(nameof(error));
            }
        }

        private IndexObjectsResult IndexDataChunkInParts(IReadOnlyList<object> objectsWithErrors, string indexName, string typeName, int partsQuantity)
        {
            var chunkDataParts = objectsWithErrors.Split(partsQuantity).ToList();
            var i = 0;

            Func<string, string> logWithIndexDescription =
                    description => _logger.LogWithIndexDescription(indexName, typeName, description);

            _logger.Warn(logWithIndexDescription($"Parted chunk in {partsQuantity} parts: { Join("-", chunkDataParts.Select(cp => $"Part {++i}:{cp.Count()}"))}".TrimEnd('-')));

            var documentsIndexed = new List<object>();
            var documentsNotIndexed = new List<object>();

            i = 0;

            foreach (var chunkData in chunkDataParts)
            {
                var indexDocumentsResult = _objectsIndexer.Index(chunkData.ToList(), indexName, typeName);

                if (indexDocumentsResult.HaveError)
                {
                    _objectsStore.StoreObjects(objectsWithErrors, indexName, typeName);
                    documentsNotIndexed.AddRange(indexDocumentsResult.ObjectsNotIndexed);
                }

                documentsIndexed.AddRange(indexDocumentsResult.ObjectsIndexed);

                _logger.Warn(logWithIndexDescription($"Part {++i}: Indexed:{indexDocumentsResult.ObjectsIndexed} - Not Indexed:{indexDocumentsResult.ObjectsNotIndexed}"));
            }

            return new IndexObjectsResult(documentsIndexed, documentsNotIndexed);
        }
    }
}
