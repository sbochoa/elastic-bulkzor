using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace Bulkzor
{
    public class Bulkzor<T>
        where T:class
    {
        private readonly Uri[] _nodes;
        private readonly ISource<T> _source;
        private readonly string _indexName;
        private readonly string _typeName;

        private Bulkzor(Uri[] nodes, ISource<T> source, string indexName, string typeName)
        {
            _nodes = nodes;
            _source = source;
            _indexName = indexName;
            _typeName = typeName;
        }

        public static Bulkzor<T> ConfigureWith(Action<BulkzorConfiguration<T>> configuration)
        {
            var bulkConfiguration = new BulkzorConfiguration<T>();
            configuration(bulkConfiguration);

            return new Bulkzor<T>(bulkConfiguration.GetNodes
                ,(ISource<T>)bulkConfiguration.GetSource
                , bulkConfiguration.GetIndexName
                , bulkConfiguration.GetTypeName);
        }  

        public void RunBulk()
        {
            var pool = new StaticConnectionPool(_nodes);
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);

            client.Bulk(b => b.IndexMany(_source.GetData(), (descriptor, arg2) => descriptor.Index(_indexName).Type(_typeName) ));
        }

        public async Task RunBulkAsync()
        {
            var pool = new StaticConnectionPool(_nodes);
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);

            await client.BulkAsync(b => b.IndexMany(_source.GetData(), (descriptor, arg2) => descriptor.Index(_indexName).Type(_typeName)));
        }
    }
}
