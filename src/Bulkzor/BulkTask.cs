﻿using System;
using Bulkzor.Configuration;
using Bulkzor.Indexers;
using Bulkzor.Processors;
using Bulkzor.Storage;
using Common.Logging;
using Elasticsearch.Net;
using Nest;

namespace Bulkzor
{
    public class BulkTask : IBulkTask
    {
        private readonly BulkTaskConfiguration _bulkTaskConfiguration;
        private readonly ChunkConfiguration _chunkConfiguration;
        private readonly ISource _source;
        private readonly ILog _logger;

        public BulkTaskConfiguration BulkTaskConfiguration => _bulkTaskConfiguration;

        public BulkTask(BulkTaskConfiguration bulkTaskConfiguration, 
                        ChunkConfiguration chunkConfiguration,
                        ISource source, 
                        ILog logger)
        {
            _bulkTaskConfiguration = bulkTaskConfiguration;
            _chunkConfiguration = chunkConfiguration;
            _source = source;
            _logger = logger;
        }

        public void Run()
        {
            try
            {
                var source = _source as IManagedSource;

                source?.OpenConnection();

                var documentProcessor = CreateDocumentProcessor(_bulkTaskConfiguration.GetFullHost(), _bulkTaskConfiguration.TaskName);

                var result = documentProcessor.IndexData(_source.GetData()
                                                    , _bulkTaskConfiguration.GetIndexNameBuilder()
                                                    , _bulkTaskConfiguration.TypeName);

                source?.CloseConnection();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

         }

        public DataProcessor CreateDocumentProcessor(string host, string taskName)
        {
            var pool = new StaticConnectionPool(new []{ new Uri(host) });
            var settings = new ConnectionSettings(pool);
            var elasticClient = new ElasticClient(settings);
            var objectIndexer = new NestObjectsIndexer(elasticClient, _logger);

            return new DataProcessor
                (new ChunkProcessor(_chunkConfiguration
                                    , objectIndexer
                                    , new InFileObjectsStorage(taskName)
                                    , _logger)
                , _logger);
        }
    }
}
