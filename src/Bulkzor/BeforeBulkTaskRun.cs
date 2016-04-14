using Nest;

namespace Bulkzor
{
    public delegate void BeforeBulkTaskRun(ElasticClient client, string indexName, string typeName);
}