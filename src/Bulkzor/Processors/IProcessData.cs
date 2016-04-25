using System;
using System.Collections.Generic;
using Bulkzor.Configuration;
using Bulkzor.Results;

namespace Bulkzor.Processors
{
    public interface IProcessData<T>
        where T : class, IIndexableObject
    {
        ObjectsProcessedResult IndexData(IEnumerable<T> data, Func<T, string> indexNameFunc, string typeName, int chunkSize);
    }
}
