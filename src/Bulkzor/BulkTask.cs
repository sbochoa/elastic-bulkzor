using System;
using Bulkzor.Callbacks;
using Bulkzor.Commands;
using Bulkzor.Configuration;
using Bulkzor.Handlers;
using Bulkzor.Indexers;
using Bulkzor.Results;

namespace Bulkzor
{
    public class BulkTask<T> : IBulkTask
        where T:class
    {
        private readonly ISource<T> _source;
        private readonly string _indexName;
        private readonly string _typeName;
        private readonly BeforeBulkTaskRun _beforeBulkTaskRun;
        private readonly AfterBulkTaskRun _afterBulkTaskRun;
        private readonly OnBulkTaskError _onBulkTaskError;
        private readonly ChunkConfiguration _chunkConfiguration;
        private readonly IHandle<BulkDocuments<T>, IndexResult> _bulkDocumentsHandler;
        private readonly IDocumentsIndexer _documentIndexer;

        private BulkTask(ISource<T> source, string indexName, string typeName, BeforeBulkTaskRun beforeBulkTaskRun
            , AfterBulkTaskRun afterBulkTaskRun, OnBulkTaskError onBulkTaskError
            , ChunkConfiguration chunkConfiguration
            , IHandle<BulkDocuments<T>, IndexResult> bulkDocumentsHandler
            , IDocumentsIndexer documentIndexer)
        {
            _source = source;
            _indexName = indexName;
            _typeName = typeName;
            _beforeBulkTaskRun = beforeBulkTaskRun;
            _afterBulkTaskRun = afterBulkTaskRun;
            _onBulkTaskError = onBulkTaskError;
            _chunkConfiguration = chunkConfiguration;
            _bulkDocumentsHandler = bulkDocumentsHandler;
            _documentIndexer = documentIndexer;
        }

        public static BulkTask<T> ConfigureWith(Action<BulkTaskConfiguration<T>> configuration)
        {
            var bulkConfiguration = new BulkTaskConfiguration<T>();
            configuration(bulkConfiguration);

            return new BulkTask<T>((ISource<T>)bulkConfiguration.GetSource
                , bulkConfiguration.GetIndexName 
                , bulkConfiguration.GetTypeName
                , bulkConfiguration.GetBeforeBulkTaskRun
                , bulkConfiguration.GetAfterBulkTaskRun
                , bulkConfiguration.GetOnBulkTaskError
                , bulkConfiguration.ChunkConfiguration
                , bulkConfiguration.GetBulkDocumentsHandler
                , bulkConfiguration.GetDocumentIndexer);
        }  

        public void Run()
        {
            try
            {
                _beforeBulkTaskRun?.Invoke(_documentIndexer, _indexName, _typeName);

                var source = _source as IManagedSource<T>;

                source?.OpenConnection();

                var result = 
                    _bulkDocumentsHandler
                        .Handle(new BulkDocuments<T>(_source.GetData(), _indexName, _typeName, _chunkConfiguration));

                source?.CloseConnection();

                _afterBulkTaskRun?.Invoke(_documentIndexer, _indexName, _typeName, result);
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
