using System;
using Bulkzor.Results;

namespace Bulkzor.Callbacks
{
    public delegate void OnChunkIndexed(DocumentIndexingResult result, string indexName, string typeName, TimeSpan timeElapsed);
}