using System;
using System.Collections.Generic;
using Bulkzor.Configuration;
using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IIndexData
    {
        IndexResult IndexData<T>(IEnumerable<T> data, Func<T, string> indexNameFunc, string typeName, ChunkConfiguration chunkConfiguration) where T : class;
    }
}
