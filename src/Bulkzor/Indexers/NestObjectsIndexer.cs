﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Results;
using Bulkzor.Types;
using Bulkzor.Utils;
using Common.Logging;
using Nest;

namespace Bulkzor.Indexers
{
    public class NestObjectsIndexer
        : IIndexObjects
    {
        private readonly ElasticClient _client;
        private readonly ILog _logger;
        public ElasticClient Client => _client;

        public NestObjectsIndexer(ElasticClient client, ILog logger)
        {
            _client = client;
            _logger = logger;
        }

        public IndexObjectsResult<T> Index<T>(IReadOnlyList<T> objects, string indexName, string typeName)
            where T : class, IIndexableObject
        {
            IBulkResponse response = _client.IndexMany(objects, indexName, typeName);

            IndexingError indexingError;

            Func<string, string> logWithIndexDescription =
                    description => _logger.LogWithIndexDescription(indexName, typeName, description);

            // for now this is the only way we can at least guess it was a Length exceeded error
            // so it worth the try to chunk in parts when this happens
            if (!response.ApiCall.Success)
            {
                indexingError = IndexingError.LengthExceeded;
                _logger.Error(logWithIndexDescription("Length exceeded exception ocurred in the server"));
                _logger.Error(logWithIndexDescription(response.ApiCall.ServerError.Error.Reason));

                return new IndexObjectsResult<T>(new List<T>(), objects, indexingError);
            }
            else if (response.ItemsWithErrors?.Any() ?? false)
            {
                indexingError = IndexingError.OnlyPartOfDocumentsIndexed;

                foreach (var itemWithError in response.ItemsWithErrors)
                {
                    _logger.Error(logWithIndexDescription("Only part of the items were indexed"));
                    _logger.Error(logWithIndexDescription($"Id:{itemWithError.Id} - Error:{itemWithError.Error.Reason}"));
                }
            }
            else if (response.Errors)
            {
                indexingError = IndexingError.Unknow;
                _logger.Error(logWithIndexDescription(response.ServerError.Error.Reason));
            }
            else
            {
                indexingError = IndexingError.None;
                _logger.Error(logWithIndexDescription(response.ServerError.Error.Reason));
            }

            var documentsIndexed = objects.Where(d => response.Items?.Any(i => i.Id == d.Id) ?? false).ToList();
            var documentsNotIndexed = objects.Where(d => response.ItemsWithErrors?.Any(i => i.Id == d.Id) ?? false).ToList();

            return new IndexObjectsResult<T>(documentsIndexed, documentsNotIndexed, indexingError);
        }
    }
}