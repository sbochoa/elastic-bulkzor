using Bulkzor.Results;

namespace Bulkzor.Indexers
{
    public interface IIndexDataChunk
    {
        IndexResult IndexDataChunk<T>(DataChunk<T> dataChunk) where T : class;
    }
}