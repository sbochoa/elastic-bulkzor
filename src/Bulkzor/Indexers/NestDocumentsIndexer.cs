﻿using System.Collections.Generic;
using System.Linq;
using Bulkzor.Results;
using Bulkzor.Types;
using Nest;

namespace Bulkzor.Indexers
{
    public class NestDocumentsIndexer
        : IIndexDocuments
    {
        private readonly ElasticClient _client;
        public ElasticClient Client => _client;

        public NestDocumentsIndexer(ElasticClient client)
        {
            _client = client;
        }

        public IndexDocumentsResult IndexDocuments<T>(IEnumerable<T> documents, string indexName, string typeName)
            where T : class  
        {
            IBulkResponse response = _client.IndexMany(documents, indexName, typeName);

            IndexingError indexingError;

            // for now this is the only way we can at least guess it was a Length exceeded error
            // so it worth the try to chunk in parts when this happens
            if (!response.ApiCall.Success)
            {
                indexingError = IndexingError.LengthExceeded;
            }
            else if (response.ItemsWithErrors.Any())
            {
                indexingError = IndexingError.OnlyPartOfDocumentsIndexed;
            }
            else if (response.Errors)
            {
                indexingError = IndexingError.Unknow;
            }
            else
            {
                indexingError = IndexingError.None;
            }

            var documentsIndexed = 0;
            var documentsNotIndexed = 0;

            if (response.Items != null)
            {
                documentsIndexed = response.Items.Count();
            }
            else
            {
                documentsIndexed = 0;
            }

            if (response.ItemsWithErrors != null)
            {
                documentsNotIndexed = response.ItemsWithErrors.Count();
            }
            else
            {
                documentsNotIndexed = 0;
            }

            return new IndexDocumentsResult(documentsIndexed, documentsNotIndexed, indexingError);
        }
    }
}
