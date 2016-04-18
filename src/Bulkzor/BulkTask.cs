using System;
using Bulkzor.Callbacks;
using Bulkzor.Configuration;
using Bulkzor.Indexers;

namespace Bulkzor
{
    public class BulkTask<TDocument> : IBulkTask
        where TDocument:class
    {
        private readonly ISource<TDocument> _source;
        private readonly string _indexName;
        private readonly string _typeName;
        private readonly BeforeBulkTaskRun _beforeBulkTaskRun;
        private readonly AfterBulkTaskRun _afterBulkTaskRun;
        private readonly OnBulkTaskError _onBulkTaskError;
        private readonly ChunkConfiguration _chunkConfiguration;
        private readonly IIndexDocuments _documentIndexer;
        private readonly IIndexData _dataIndexor;

        private BulkTask(ISource<TDocument> source, string indexName, string typeName, BeforeBulkTaskRun beforeBulkTaskRun
            , AfterBulkTaskRun afterBulkTaskRun, OnBulkTaskError onBulkTaskError
            , ChunkConfiguration chunkConfiguration, IIndexDocuments documentIndexer, IIndexData dataIndexor)
        {
            _source = source;
            _indexName = indexName;
            _typeName = typeName;
            _beforeBulkTaskRun = beforeBulkTaskRun;
            _afterBulkTaskRun = afterBulkTaskRun;
            _onBulkTaskError = onBulkTaskError;
            _chunkConfiguration = chunkConfiguration;
            _documentIndexer = documentIndexer;
            _dataIndexor = dataIndexor;
        }

        public static BulkTask<TDocument> ConfigureWith(Action<BulkTaskConfiguration<TDocument>> configuration)
        {
            var bulkConfiguration = new BulkTaskConfiguration<TDocument>();
            configuration(bulkConfiguration);

            return new BulkTask<TDocument>((ISource<TDocument>)bulkConfiguration.GetSource
                , bulkConfiguration.GetIndexName 
                , bulkConfiguration.GetTypeName
                , bulkConfiguration.GetBeforeBulkTaskRun
                , bulkConfiguration.GetAfterBulkTaskRun
                , bulkConfiguration.GetOnBulkTaskError
                , bulkConfiguration.ChunkConfiguration
                , bulkConfiguration.GetDocumentIndexer
                , bulkConfiguration.GetDataIndexer);
        }  

        public void Run()
        {
            try
            {
                _beforeBulkTaskRun?.Invoke(_documentIndexer, _indexName, _typeName);

                var source = _source as IManagedSource<TDocument>;

                source?.OpenConnection();

                var result = _dataIndexor.IndexData(_source.GetData(), _indexName, _typeName, _chunkConfiguration);

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
