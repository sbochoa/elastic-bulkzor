using System;
using System.Collections.Generic;
using Bulkzor.Callbacks;
using Bulkzor.Configuration;
using Bulkzor.Indexers;

namespace Bulkzor
{
    public class BulkTask<T> : IBulkTask
        where T:class
    {
        private readonly ISource _source;
        private readonly IEnumerable<T> _data;
        private readonly Func<T, string> _indexNameFunc;
        private readonly string _typeName;
        private readonly BeforeBulkTaskRun _beforeBulkTaskRun;
        private readonly AfterBulkTaskRun _afterBulkTaskRun;
        private readonly OnBulkTaskError _onBulkTaskError;
        private readonly ChunkConfiguration _chunkConfiguration;
        private readonly IIndexData _dataIndexor;

        private BulkTask(ISource source, IEnumerable<T> data, Func<T, string> indexNameFunc, string typeName, BeforeBulkTaskRun beforeBulkTaskRun
            , AfterBulkTaskRun afterBulkTaskRun, OnBulkTaskError onBulkTaskError
            , ChunkConfiguration chunkConfiguration, IIndexData dataIndexor)
        {
            _source = source;
            _data = data;
            _indexNameFunc = indexNameFunc;
            _typeName = typeName;
            _beforeBulkTaskRun = beforeBulkTaskRun;
            _afterBulkTaskRun = afterBulkTaskRun;
            _onBulkTaskError = onBulkTaskError;
            _chunkConfiguration = chunkConfiguration;
            _dataIndexor = dataIndexor;
        }

        public static BulkTask<T> ConfigureWith(Action<BulkTaskConfiguration<T>> configuration)
        {
            var bulkConfiguration = new BulkTaskConfiguration<T>();
            configuration(bulkConfiguration);

            return new BulkTask<T>(bulkConfiguration.GetSource
                , bulkConfiguration.GetData
                , bulkConfiguration.GetIndexNameFunc
                , bulkConfiguration.GetTypeName
                , bulkConfiguration.GetBeforeBulkTaskRun
                , bulkConfiguration.GetAfterBulkTaskRun
                , bulkConfiguration.GetOnBulkTaskError
                , bulkConfiguration.ChunkConfiguration
                , bulkConfiguration.GetDataIndexer);
        }  

        public void Run()
        {
            try
            {
                _beforeBulkTaskRun?.Invoke();

                var source = _source as IManagedSource;

                source?.OpenConnection();

                var result = _dataIndexor.IndexData(_data ?? _source.GetData<T>(), _indexNameFunc, _typeName, _chunkConfiguration);

                source?.CloseConnection();

                _afterBulkTaskRun?.Invoke(result);
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
