using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IIndexDataChunk
    {
        IndexDataChunkResult IndexDataChunk<T>(DataChunk<T> dataChunk) where T : class;
    }
}