using System.Collections.Generic;
using Bulkzor.Models;
using Bulkzor.Results;

namespace Bulkzor.Processors
{
    public interface IProcessChunks<T>
        where T : class, IIndexableObject
    {
        ObjectsProcessedResult ProcessChunks(IReadOnlyList<Chunk<T>> chunks);
    }
}