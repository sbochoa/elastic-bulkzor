using System;
using Bulkzor.Indexers;

namespace Bulkzor.Callbacks
{
    public delegate void AfterBulkTaskRun(IDocumentsIndexer documentsIndexer, string indexName, string typeName, int documentsIndexedCount, TimeSpan timeElapsed);
}