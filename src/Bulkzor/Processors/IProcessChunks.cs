using System.Collections.Generic;
using Bulkzor.Models;
using Bulkzor.Results;

namespace Bulkzor.Processors
{
    public interface IProcessChunks
    {
        ObjectsProcessedResult ProcessChunks<T>(IReadOnlyList<Chunk<T>> chunks)
            where T : class, IIndexableObject;
    }
}