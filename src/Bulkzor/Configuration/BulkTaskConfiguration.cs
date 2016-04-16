using System;
using Bulkzor.Events;
using Elasticsearch.Net;
using Nest;

namespace Bulkzor
{
    public class BulkTaskConfiguration<T>
        where T : class
    {
        private object _source;
        private string _typeName;
        private string _indexName;
        private Uri[] _nodes;
        private BeforeBulkTaskRun _beforeBulkTaskRun;
        private AfterBulkTaskRun _afterBulkTaskRun;
        private OnBulkTaskError _onBulkTaskError;
        private IDocumentIndexer _documentIndexer;
        private ChunkConfiguration _chunkConfiguration;

        internal object GetSource => _source;
        internal string GetTypeName => _typeName ?? nameof(T);
        internal string GetIndexName => _indexName ?? $"{nameof(T)}s";
        internal Uri[] GetNodes => _nodes;
        internal AfterBulkTaskRun GetAfterBulkTaskRun => _afterBulkTaskRun;
        internal BeforeBulkTaskRun GetBeforeBulkTaskRun => _beforeBulkTaskRun;
        internal OnBulkTaskError GetOnBulkTaskError => _onBulkTaskError;
        public ChunkConfiguration ChunkConfiguration => _chunkConfiguration ?? (_chunkConfiguration = new ChunkConfiguration());

        internal IDocumentIndexer GetDocumentIndexer()
        {
            if (_documentIndexer == null)
            {
                UsingNestDocumentIndexer();
            }
            return _documentIndexer;
        }

        public BulkTaskConfiguration<T> Nodes(params Uri[] nodes)
        {
            _nodes = nodes;
            return this;
        }
        public BulkTaskConfiguration<T> Source(ISource<T> source)
        {
            _source = source;
            return this;
        }

        public BulkTaskConfiguration<T> TypeName(string typeName)
        {
            _typeName = typeName;
            return this;
        }

        public BulkTaskConfiguration<T> IndexName(string indexName)
        {
            _indexName = indexName;
            return this;
        }

        public BulkTaskConfiguration<T> BeforeBulkTaskRun(BeforeBulkTaskRun beforeBulkTaskRun)
        {
            _beforeBulkTaskRun = beforeBulkTaskRun;
            return this;
        }

        public BulkTaskConfiguration<T> AfterBulkTaskRun(AfterBulkTaskRun afterBulkTaskRun)
        {
            _afterBulkTaskRun = afterBulkTaskRun;
            return this;
        }

        public BulkTaskConfiguration<T> OnBulkTaskError(OnBulkTaskError onBulkTaskError)
        {
            _onBulkTaskError = onBulkTaskError;
            return this;
        }

        public BulkTaskConfiguration<T> UsingCustomDocumentIndexer(IDocumentIndexer documentIndexer)
        {
            _documentIndexer = documentIndexer;
            return this;
        }

        public BulkTaskConfiguration<T> UsingNestDocumentIndexer()
        {
            var pool = new StaticConnectionPool(_nodes);
            var settings = new ConnectionSettings(pool);

            var client = new ElasticClient(settings);

            _documentIndexer = new NestDocumentIndexer(client, ChunkConfiguration);
            return this;
        }
    }
}
