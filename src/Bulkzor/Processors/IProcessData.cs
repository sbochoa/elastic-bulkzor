using System;
using System.Collections.Generic;
using Bulkzor.Configuration;
using Bulkzor.Results;

namespace Bulkzor.Processors
{
    public interface IProcessData
    {
        ObjectsProcessedResult IndexData<T>(IEnumerable<T> data, Func<T, string> indexNameFunc, string typeName, int chunkSize) 
            where T : class, IIndexableObject;
    }
}
