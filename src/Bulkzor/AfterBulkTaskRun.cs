using System;
using Nest;

namespace Bulkzor
{
    public delegate void AfterBulkTaskRun(ElasticClient client, string indexName, string typeName, int documentsIndexedCount, TimeSpan timeElapsed);
}