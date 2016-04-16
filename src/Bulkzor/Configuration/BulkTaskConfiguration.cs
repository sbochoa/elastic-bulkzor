using System;
using Bulkzor.Callbacks;
using Bulkzor.Indexers;
using Elasticsearch.Net;
using Nest;

namespace Bulkzor.Configuration
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
        private IDocumentsIndexer _documentsIndexer;
        private ChunkConfiguration _chunkConfiguration;

        internal object GetSource => _source;
        internal string GetTypeName => _typeName ?? nameof(T);
        internal string GetIndexName => _indexName ?? $"{nameof(T)}s";
        internal Uri[] GetNodes => _nodes;
        internal AfterBulkTaskRun GetAfterBulkTaskRun => _afterBulkTaskRun;
        internal BeforeBulkTaskRun GetBeforeBulkTaskRun => _beforeBulkTaskRun;
        internal OnBulkTaskError GetOnBulkTaskError => _onBulkTaskError;
        public ChunkConfiguration ChunkConfiguration => _chunkConfiguration ?? (_chunkConfiguration = new ChunkConfiguration());

        internal IDocumentsIndexer GetDocumentIndexer()
        {
            if (_documentsIndexer == null)
            {
                UsingNestDocumentIndexer();
            }
            return _documentsIndexer;
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

        public BulkTaskConfiguration<T> UsingCustomDocumentIndexer(IDocumentsIndexer documentsIndexer)
        {
            _documentsIndexer = documentsIndexer;
            return this;
        }

        public BulkTaskConfiguration<T> UsingNestDocumentIndexer()
        {
            var pool = new StaticConnectionPool(_nodes);
            var settings = new ConnectionSettings(pool);

            var client = new ElasticClient(settings);

            _documentsIndexer = new NestDocumentsIndexer(client, ChunkConfiguration);
            return this;
        }
    }
}
