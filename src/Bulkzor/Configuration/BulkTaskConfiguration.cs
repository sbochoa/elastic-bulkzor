using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bulkzor.Callbacks;
using Bulkzor.Indexers;
using Elasticsearch.Net;
using Nest;

namespace Bulkzor.Configuration
{
    public class BulkTaskConfiguration<T>
        where T : class
    {
        private ISource _source;
        private string _typeName;
        private Func<T, string> _indexNameFunc;
        private Uri[] _nodes;
        private BeforeBulkTaskRun _beforeBulkTaskRun;
        private AfterBulkTaskRun _afterBulkTaskRun;
        private OnBulkTaskError _onBulkTaskError;
        private ChunkConfiguration _chunkConfiguration;
        private IIndexDocuments _documentIndexer;
        private IEnumerable<T> _data;

        internal ISource GetSource => _source;
        internal string GetTypeName => _typeName ?? nameof(T);
        internal Func<T, string> GetIndexNameFunc => _indexNameFunc ?? (@object => $"{nameof(T)}s"); 
        internal Uri[] GetNodes => _nodes;
        internal AfterBulkTaskRun GetAfterBulkTaskRun => _afterBulkTaskRun;
        internal BeforeBulkTaskRun GetBeforeBulkTaskRun => _beforeBulkTaskRun;
        internal OnBulkTaskError GetOnBulkTaskError => _onBulkTaskError;
        public ChunkConfiguration ChunkConfiguration => _chunkConfiguration ?? (_chunkConfiguration = new ChunkConfiguration());
        public IIndexDocuments GetDocumentIndexer => _documentIndexer ?? (_documentIndexer = CreateNestDocumentIndexer());


        internal IIndexData GetDataIndexer => 
            new DataIndexer
                (new DataChunkIndexer(GetDocumentIndexer, ChunkConfiguration.GetBeforeIndexDataChunk, ChunkConfiguration.GetAfterIndexDataChunk));
        public IEnumerable<T> GetData => _data;

        public BulkTaskConfiguration<T> Nodes(params Uri[] nodes)
        {
            _nodes = nodes;
            return this;
        }
        public BulkTaskConfiguration<T> Source(ISource source)
        {
            _source = source;
            return this;
        }

        public BulkTaskConfiguration<T> Source(IEnumerable<T> data)
        {
            _data = data;
            return this;
        }

        public BulkTaskConfiguration<T> IndexName(Func<T, string> indexNameFunc)
        {
            _indexNameFunc = indexNameFunc;
            return this;
        }

        public BulkTaskConfiguration<T> TypeName(string typeName)
        {
            _typeName = typeName;
            return this;
        }

        public BulkTaskConfiguration<T> IndexName(string indexName)
        {
            _indexNameFunc = @object => indexName;
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

        public BulkTaskConfiguration<T> UsingCustomDocumentIndexer(IIndexDocuments documentsIndexer)
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
