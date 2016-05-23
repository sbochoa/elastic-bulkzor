using System;
using System.Collections.Generic;
using Bulkzor.Callbacks;
using Bulkzor.Configuration;
using Bulkzor.Processors;

namespace Bulkzor
{
    public class BulkTask : IBulkTask
    {
        private readonly ISource _source;
        private readonly IEnumerable<dynamic> _data;
        private readonly Func<dynamic, string> _indexNameFunc;
        private readonly string _typeName;
        private readonly BeforeBulkTaskRun _beforeBulkTaskRun;
        private readonly AfterBulkTaskRun _afterBulkTaskRun;
        private readonly OnBulkTaskError _onBulkTaskError;
        private readonly ChunkConfiguration _chunkConfiguration;
        private readonly IProcessData _dataProcessor;

        private BulkTask(ISource source, IEnumerable<dynamic> data, Func<dynamic, string> indexNameFunc, string typeName, BeforeBulkTaskRun beforeBulkTaskRun
            , AfterBulkTaskRun afterBulkTaskRun, OnBulkTaskError onBulkTaskError
            , ChunkConfiguration chunkConfiguration, IProcessData dataProcessor)
        {
            _source = source;
            _data = data;
            _indexNameFunc = indexNameFunc;
            _typeName = typeName;
            _beforeBulkTaskRun = beforeBulkTaskRun;
            _afterBulkTaskRun = afterBulkTaskRun;
            _onBulkTaskError = onBulkTaskError;
            _chunkConfiguration = chunkConfiguration;
            _dataProcessor = dataProcessor;
        }

        public static BulkTask ConfigureWith(Action<BulkTaskConfiguration> configuration)
        {
            var bulkConfiguration = new BulkTaskConfiguration();
            configuration(bulkConfiguration);

            return new BulkTask(bulkConfiguration.GetSource
                , bulkConfiguration.GetData
                , bulkConfiguration.GetIndexNameFunc
                , bulkConfiguration.GetTypeName
                , bulkConfiguration.GetBeforeBulkTaskRun
                , bulkConfiguration.GetAfterBulkTaskRun
                , bulkConfiguration.GetOnBulkTaskError
                , bulkConfiguration.GetChunkConfiguration
                , bulkConfiguration.GetDataIndexer);
        }  

        public void Run()
        {
            try
            {
                _beforeBulkTaskRun?.Invoke();

                var source = _source as IManagedSource;

                source?.OpenConnection();

                var result = _dataProcessor.IndexData(_data ?? _source.GetData(), _indexNameFunc, _typeName, _chunkConfiguration.GetChunkSize);

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
