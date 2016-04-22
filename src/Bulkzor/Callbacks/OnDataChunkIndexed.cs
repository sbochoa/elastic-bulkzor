using System;
using Bulkzor.Results;

namespace Bulkzor.Callbacks
{
    public delegate void OnDataChunkIndexed(IndexDataChunkResult result, string indexName, string typeName);
}