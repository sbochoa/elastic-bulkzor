using System;
using System.Collections.Generic;
using Bulkzor.Configuration;
using Bulkzor.Results;

namespace Bulkzor.Processors
{
    public interface IProcessData
    {
        ObjectsProcessedResult IndexData(IEnumerable<object> data, Func<object, string> indexNameFunc, string typeName, int chunkSize);
    }
}
