using System;

namespace Bulkzor
{
    public delegate void OnBulkIndexed(int documentsIndexedCount, string indexName, string typeName, TimeSpan timeElapsed);
}