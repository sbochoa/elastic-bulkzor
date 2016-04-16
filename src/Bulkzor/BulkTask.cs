using System;
using Bulkzor.Callbacks;
using Bulkzor.Configuration;
using Bulkzor.Indexers;

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
        private readonly IDocumentsIndexer _documentsIndexer;

        private BulkTask(ISource<T> source, string indexName, string typeName, BeforeBulkTaskRun beforeBulkTaskRun, AfterBulkTaskRun afterBulkTaskRun, OnBulkTaskError onBulkTaskError, IDocumentsIndexer documentsIndexer)
        {
            _source = source;
            _indexName = indexName;
            _typeName = typeName;
            _beforeBulkTaskRun = beforeBulkTaskRun;
            _afterBulkTaskRun = afterBulkTaskRun;
            _onBulkTaskError = onBulkTaskError;
            _documentsIndexer = documentsIndexer;
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
                , bulkConfiguration.GetDocumentIndexer());
        }  

        public void Run()
        {
            try
            {
                _beforeBulkTaskRun?.Invoke(_documentsIndexer, _indexName, _typeName);

                var source = _source as IManagedSource<T>;

                source?.OpenConnection();

                var result = _documentsIndexer.Index(_source.GetData(), _indexName, _typeName);

                source?.CloseConnection();

                _afterBulkTaskRun?.Invoke(_documentsIndexer, _indexName, _typeName, result.TotalDocumentsIndexed, result.TimeElapsed);
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
