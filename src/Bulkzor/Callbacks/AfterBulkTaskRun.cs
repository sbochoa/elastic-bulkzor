using System;
using Bulkzor.Indexers;
using Bulkzor.Results;

namespace Bulkzor.Callbacks
{
    public delegate void AfterBulkTaskRun(IDocumentsIndexer documentsIndexer, string indexName, string typeName, IndexResult result);
}