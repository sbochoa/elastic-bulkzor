using System;
using Bulkzor.Callbacks;
using Bulkzor.Commands;
using Bulkzor.Handlers;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Elasticsearch.Net;
using Nest;

namespace Bulkzor.Configuration
{
    public class BulkTaskConfiguration<TDocument>
        where TDocument : class
    {
        private object _source;
        private string _typeName;
        private string _indexName;
        private Uri[] _nodes;
        private BeforeBulkTaskRun _beforeBulkTaskRun;
        private AfterBulkTaskRun _afterBulkTaskRun;
        private OnBulkTaskError _onBulkTaskError;
        private ChunkConfiguration _chunkConfiguration;
        private IDocumentsIndexer _documentIndexer;

        internal object GetSource => _source;
        internal string GetTypeName => _typeName ?? nameof(TDocument);
        internal string GetIndexName => _indexName ?? $"{nameof(TDocument)}s";
        internal Uri[] GetNodes => _nodes;
        internal AfterBulkTaskRun GetAfterBulkTaskRun => _afterBulkTaskRun;
        internal BeforeBulkTaskRun GetBeforeBulkTaskRun => _beforeBulkTaskRun;
        internal OnBulkTaskError GetOnBulkTaskError => _onBulkTaskError;
        public ChunkConfiguration ChunkConfiguration => _chunkConfiguration ?? (_chunkConfiguration = new ChunkConfiguration());

        internal IDocumentsIndexer GetDocumentIndexer => _documentIndexer ?? (_documentIndexer = CreateNestDocumentIndexer());

        internal IHandle<BulkDocuments<TDocument>, IndexResult> GetBulkDocumentsHandler =>
            new BulkDocumentsHandler<TDocument>
                (new IndexChunkHandler<TDocument>
                    (GetDocumentIndexer, new IndexChunkInPartsHandler<TDocument>(GetDocumentIndexer), new StoreDocumentsNotIndexedHandler<TDocument>()));

        public BulkTaskConfiguration<TDocument> Nodes(params Uri[] nodes)
        {
            _nodes = nodes;
            return this;
        }
        public BulkTaskConfiguration<TDocument> Source(ISource<TDocument> source)
        {
            _source = source;
            return this;
        }

        public BulkTaskConfiguration<TDocument> TypeName(string typeName)
        {
            _typeName = typeName;
            return this;
        }

        public BulkTaskConfiguration<TDocument> IndexName(string indexName)
        {
            _indexName = indexName;
            return this;
        }

        public BulkTaskConfiguration<TDocument> BeforeBulkTaskRun(BeforeBulkTaskRun beforeBulkTaskRun)
        {
            _beforeBulkTaskRun = beforeBulkTaskRun;
            return this;
        }

        public BulkTaskConfiguration<TDocument> AfterBulkTaskRun(AfterBulkTaskRun afterBulkTaskRun)
        {
            _afterBulkTaskRun = afterBulkTaskRun;
            return this;
        }

        public BulkTaskConfiguration<TDocument> OnBulkTaskError(OnBulkTaskError onBulkTaskError)
        {
            _onBulkTaskError = onBulkTaskError;
            return this;
        }

        public BulkTaskConfiguration<TDocument> UsingCustomDocumentIndexer(IDocumentsIndexer documentsIndexer)
        {
            _documentIndexer = documentsIndexer;
            return this;
        }

        private NestDocumentsIndexer CreateNestDocumentIndexer()
        {
            var pool = new StaticConnectionPool(_nodes);
            var settings = new ConnectionSettings(pool);

            var client = new ElasticClient(settings);

            return new NestDocumentsIndexer(client);
        }
    }
}
