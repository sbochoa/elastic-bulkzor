using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Elasticsearch.Net;
using Nest;

namespace Bulkzor
{
    public class BulkTask<T> : IBulkTask
        where T:class
    {
        private readonly Uri[] _nodes;
        private readonly ISource<T> _source;
        private readonly string _indexName;
        private readonly string _typeName;
        private readonly int _bulkSize;
        private readonly BeforeBulkTaskRun _beforeBulkTaskRun;
        private readonly AfterBulkTaskRun _afterBulkTaskRun;
        private readonly OnBulkTaskError _onBulkTaskError;
        private readonly OnBulkIndexed _onBulkIndexed;

        private BulkTask(Uri[] nodes, ISource<T> source, string indexName, string typeName, int bulkSize
            , BeforeBulkTaskRun beforeBulkTaskRun, AfterBulkTaskRun afterBulkTaskRun, OnBulkTaskError onBulkTaskError, OnBulkIndexed onBulkIndexed)
        {
            _nodes = nodes;
            _source = source;
            _indexName = indexName;
            _typeName = typeName;
            _bulkSize = bulkSize;
            _beforeBulkTaskRun = beforeBulkTaskRun;
            _afterBulkTaskRun = afterBulkTaskRun;
            _onBulkTaskError = onBulkTaskError;
            _onBulkIndexed = onBulkIndexed;
        }

        public static BulkTask<T> ConfigureWith(Action<BulkTaskConfiguration<T>> configuration)
        {
            var bulkConfiguration = new BulkTaskConfiguration<T>();
            configuration(bulkConfiguration);

            return new BulkTask<T>(bulkConfiguration.GetNodes
                ,(ISource<T>)bulkConfiguration.GetSource
                , bulkConfiguration.GetIndexName
                , bulkConfiguration.GetTypeName
                , bulkConfiguration.GetBulkSize == 0 ? 1000 : bulkConfiguration.GetBulkSize
                , bulkConfiguration.GetBeforeBulkTaskRun
                , bulkConfiguration.GetAfterBulkTaskRun
                , bulkConfiguration.GetOnBulkTaskError
                , bulkConfiguration.GetOnBulkIndexed);
        }  

        public void Run()
        {
            try
            {
                var pool = new StaticConnectionPool(_nodes);
                var settings = new ConnectionSettings(pool);
                var client = new ElasticClient(settings);

                var startFrom = 0;
                var documentsIndexed = 0;

                var workingResults = new List<T>();

                _beforeBulkTaskRun?.Invoke(client, _indexName, _typeName);

                var source = _source as IManagedSource<T>;

                source?.OpenConnection();

                foreach (var workingResult in _source.GetData())
                {
                    if (startFrom > _bulkSize)
                    {
                        startFrom = 0;

                        client.IndexMany(workingResults, _indexName, _typeName);

                        _onBulkIndexed?.Invoke(workingResults.Count, _indexName, _typeName);

                        documentsIndexed += workingResults.Count;

                        workingResults.Clear();
                    }

                    workingResults.Add(workingResult);

                    startFrom++;
                }

                client.IndexMany(workingResults, _indexName, _typeName);

                documentsIndexed += workingResults.Count;

                source?.CloseConnection();

                workingResults.Clear();

                _afterBulkTaskRun?.Invoke(client, _indexName, _typeName, documentsIndexed);
            }
            catch (Exception ex)
            {
                if (_onBulkTaskError == null)
                {
                    throw;
                }

                _onBulkTaskError?.Invoke(ex);
            }
        }
    }
}
