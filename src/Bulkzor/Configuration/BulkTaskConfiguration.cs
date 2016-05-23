using System;
using System.Collections.Generic;
using System.Threading;
using Bulkzor.Callbacks;
using Bulkzor.Errors;
using Bulkzor.Indexers;
using Bulkzor.Processors;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.NLog;
using Elasticsearch.Net;
using Nest;
using NLog.Config;
using NLog.Targets;
using LogLevel = NLog.LogLevel;

namespace Bulkzor.Configuration
{
    public class BulkTaskConfiguration
    {
        private ISource _source;
        private string _typeName;
        private Func<object, string> _indexNameFunc;
        private Uri[] _nodes;
        private BeforeBulkTaskRun _beforeBulkTaskRun;
        private AfterBulkTaskRun _afterBulkTaskRun;
        private OnBulkTaskError _onBulkTaskError;
        private ChunkConfiguration _chunkConfiguration;
        private IIndexObjects _documentIndexer;
        private IEnumerable<object> _data;
        private ILog _logger;
        private string _name;

        internal ISource GetSource => _source;
        internal string GetTypeName => _typeName;
        internal Func<object, string> GetIndexNameFunc => _indexNameFunc; 
        internal AfterBulkTaskRun GetAfterBulkTaskRun => _afterBulkTaskRun;
        internal BeforeBulkTaskRun GetBeforeBulkTaskRun => _beforeBulkTaskRun;
        internal OnBulkTaskError GetOnBulkTaskError => _onBulkTaskError;
        internal ChunkConfiguration GetChunkConfiguration => _chunkConfiguration ?? (_chunkConfiguration = new ChunkConfiguration());
        public IIndexObjects GetDocumentIndexer => _documentIndexer ?? (_documentIndexer = CreateNestDocumentIndexer());


        internal IProcessData GetDataIndexer => 
            new DataProcessor
                (new ChunkProcessor
                    (GetDocumentIndexer, new IndexErrorsHandler(GetDocumentIndexer, null, GetLogger),  GetLogger)
                , GetLogger);
        public IEnumerable<object> GetData => _data;
        public ILog GetLogger => _logger ?? (_logger = LogManager.GetLogger(_name ?? $"Task-Thread:{Thread.CurrentThread.ManagedThreadId}"));

        public BulkTaskConfiguration TaskName(string name)
        {
            _name = name;
            return this;
        }

        public BulkTaskConfiguration Nodes(params Uri[] nodes)
        {
            _nodes = nodes;
            return this;
        }
        public BulkTaskConfiguration Source(ISource source)
        {
            _source = source;
            return this;
        }

        public BulkTaskConfiguration Source(IEnumerable<object> data)
        {
            _data = data;
            return this;
        }

        public BulkTaskConfiguration IndexName(Func<object, string> indexNameFunc)
        {
            _indexNameFunc = indexNameFunc;
            return this;
        }

        public BulkTaskConfiguration TypeName(string typeName)
        {
            _typeName = typeName;
            return this;
        }

        public BulkTaskConfiguration IndexName(string indexName)
        {
            _indexNameFunc = @object => indexName;
            return this;
        }

        public BulkTaskConfiguration BeforeBulkTaskRun(BeforeBulkTaskRun beforeBulkTaskRun)
        {
            _beforeBulkTaskRun = beforeBulkTaskRun;
            return this;
        }

        public BulkTaskConfiguration AfterBulkTaskRun(AfterBulkTaskRun afterBulkTaskRun)
        {
            _afterBulkTaskRun = afterBulkTaskRun;
            return this;
        }

        public BulkTaskConfiguration OnBulkTaskError(OnBulkTaskError onBulkTaskError)
        {
            _onBulkTaskError = onBulkTaskError;
            return this;
        }

        public BulkTaskConfiguration UsingCustomDocumentIndexer(IIndexObjects documentsIndexer)
        {
            _documentIndexer = documentsIndexer;
            return this;
        }

        public BulkTaskConfiguration CustomLogger(ILog logger)
        {
            _logger = logger;
            return this;
        }

        public BulkTaskConfiguration ChunkConfiguration(ChunkConfiguration chunkConfiguration)
        {
            _chunkConfiguration = chunkConfiguration;
            return this;
        }

        private NestObjectsIndexer CreateNestDocumentIndexer()
        {
            var pool = new StaticConnectionPool(_nodes);
            var settings = new ConnectionSettings(pool);

            var client = new ElasticClient(settings);

            return new NestObjectsIndexer(client, GetLogger);
        }

        private BulkTaskConfiguration NLogLogger()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            fileTarget.FileName = "${basedir}/logs/${logger}.txt";
            fileTarget.Layout = "${message}";

            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            NLog.LogManager.Configuration = config;

            var properties = new NameValueCollection { };

            LogManager.Adapter = new NLogLoggerFactoryAdapter(properties);

            return this;
        }
    }
}
