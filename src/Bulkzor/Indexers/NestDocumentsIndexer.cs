using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Results;
using Bulkzor.Types;
using Nest;

namespace Bulkzor.Indexers
{
    public class NestDocumentsIndexer
        : IDocumentsIndexer
    {
        private readonly ElasticClient _client;
        public ElasticClient Client => _client;

        public NestDocumentsIndexer(ElasticClient client)
        {
            _client = client;
        }

        public IndexResult Index<T>(IEnumerable<T> documents, string indexName, string typeName)
            where T : class 
        {
            IBulkResponse response = _client.IndexMany(documents, indexName, typeName);

            IndexingError indexingError;

            if (!response.ApiCall.Success)
            {
                indexingError = IndexingError.LengthExceeded;
            }
            else if (response.Errors)
            {
                indexingError = IndexingError.Unknow;
            }
            else
            {
                indexingError = IndexingError.None;
            }

            return new IndexResult(response.Items.Count(), response.ItemsWithErrors.Count(), TimeSpan.Zero, indexingError);
        }
    }
}
