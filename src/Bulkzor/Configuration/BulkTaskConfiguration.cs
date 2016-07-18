using System;
using System.Threading;
using SmartFormat;

namespace Bulkzor.Configuration
{
    public class BulkTaskConfiguration
    {
        private static readonly string DefaultTaskName = $"Task-Thread:{Thread.CurrentThread.ManagedThreadId}";
        public string TaskName { get; private set; }
        public string TypeName { get; set; }
        public string IndexName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public BulkTaskConfiguration(string taskName, string host, int port)
        {
            Host = host;
            Port = port;
            TaskName = taskName ?? DefaultTaskName;
        }

        public Func<object, string> GetIndexNameBuilder()
        {
            Func<object, string> indexNameBuilder = @object => Smart.Format(IndexName, @object);
            return indexNameBuilder;
        }

        public string GetFullHost()
        {
            return $"{Host}:{Port}";
        }
            
        //private ISource _source;
        //private string _typeName;
        //private Func<object, string> _indexNameBuilder;
        //private Uri[] _nodes;
        //private ChunkConfiguration _chunkConfiguration;
        //private ObjectIndexer _documentIndexer;
        //private IEnumerable<object> _data;
        //private ILog _logger;
        //private string _taskName;
        //private string _defaultTaskName => $"Task-Thread:{Thread.CurrentThread.ManagedThreadId}";

        //public string TaskName => _taskName ?? _defaultTaskName;
        //public ISource Source => _source;
        //public string TypeName => _typeName;
        //public Func<object, string> IndexNameBuilder => _indexNameBuilder;
        //public ChunkConfiguration ChunkConfiguration => _chunkConfiguration ?? (_chunkConfiguration = new ChunkConfiguration());
        //public ObjectIndexer DocumentIndexer => _documentIndexer ?? (_documentIndexer = CreateNestDocumentIndexer());
        
        //internal IEnumerable<object> Data => _data;
        //public ILog Logger => _logger ?? (_logger = LogManager.GetLogger(_taskName ?? _defaultTaskName));

        //public BulkTaskConfiguration WithTaskName(string name)
        //{
        //    _taskName = name;
        //    return this;
        //}

        //public BulkTaskConfiguration WithNodes(params Uri[] nodes)
        //{
        //    _nodes = nodes;
        //    return this;
        //}
        //public BulkTaskConfiguration WithSource(ISource source)
        //{
        //    _source = source;
        //    return this;
        //}

        //public BulkTaskConfiguration WithSource(IEnumerable<object> data)
        //{
        //    _data = data;
        //    return this;
        //}

        //public BulkTaskConfiguration WithIndexName(Func<object, string> indexNameFunc)
        //{
        //    _indexNameBuilder = indexNameFunc;
        //    return this;
        //}

        //public BulkTaskConfiguration WithTypeName(string typeName)
        //{
        //    _typeName = typeName;
        //    return this;
        //}

        //public BulkTaskConfiguration WithIndexName(string indexName)
        //{
        //    _indexNameBuilder = @object => indexName;
        //    return this;
        //}
        
        //public BulkTaskConfiguration WithCustomDocumentIndexer(ObjectIndexer documentsIndexer)
        //{
        //    _documentIndexer = documentsIndexer;
        //    return this;
        //}

        //public BulkTaskConfiguration WithCustomLogger(ILog logger)
        //{
        //    _logger = logger;
        //    return this;
        //}

        //public BulkTaskConfiguration WithChunkConfiguration(ChunkConfiguration chunkConfiguration)
        //{
        //    _chunkConfiguration = chunkConfiguration;
        //    return this;
        //}

        //private NestObjectsIndexer CreateNestDocumentIndexer()
        //{
        //    var pool = new StaticConnectionPool(_nodes);
        //    var settings = new ConnectionSettings(pool);

        //    var client = new ElasticClient(settings);

        //    return new NestObjectsIndexer(client, Logger);
        //}

        //public BulkTaskConfiguration WithNLogLogger()
        //{
        //    var config = new LoggingConfiguration();

        //    var consoleTarget = new ColoredConsoleTarget();
        //    config.AddTarget("console", consoleTarget);

        //    var fileTarget = new FileTarget();
        //    config.AddTarget("file", fileTarget);

        //    consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
        //    fileTarget.FileName = "${basedir}/logs/${logger}.txt";
        //    fileTarget.Layout = "${message}";

        //    var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
        //    config.LoggingRules.Add(rule1);

        //    var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
        //    config.LoggingRules.Add(rule2);

        //    NLog.LogManager.bulkTaskConfiguration = config;

        //    var properties = new NameValueCollection { };

        //    LogManager.Adapter = new NLogLoggerFactoryAdapter(properties);

        //    return this;
        //}
    }
}
