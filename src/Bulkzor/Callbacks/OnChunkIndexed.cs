using System;
using Bulkzor.Results;

namespace Bulkzor.Callbacks
{
    public delegate void OnChunkIndexed(IndexResult result, string indexName, string typeName);
}