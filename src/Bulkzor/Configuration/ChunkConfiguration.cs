using Bulkzor.Callbacks;

namespace Bulkzor.Configuration
{
    public class ChunkConfiguration
    {
        private int? _chunkSize;
        private int? _chunkRetries;
        private OnDataChunkIndexed _onDataChunkIndexed;
        private bool? _partChunkWhenFail;

        internal int GetChunkSize => _chunkSize ?? 1000;
        internal int GetChunkRetries => _chunkRetries ?? 5;
        internal OnDataChunkIndexed GetOnDataChunkIndexed => _onDataChunkIndexed;
        internal bool GetPartChunkWhenFail => _partChunkWhenFail ?? true;

        public ChunkConfiguration ChunkSize(int chunkSize)
        {
            _chunkSize = chunkSize;
            return this;
        }

        public ChunkConfiguration ChunkRetries(int chunkRetries)
        {
            _chunkRetries = chunkRetries;
            return this;
        }

        public ChunkConfiguration OnDataChunkIndexed(OnDataChunkIndexed onDataChunkIndexed)
        {
            _onDataChunkIndexed = onDataChunkIndexed;
            return this;
        }

        public void PartChunkWhenFail(bool partChunkWhenFail)
        {
            _partChunkWhenFail = partChunkWhenFail;
        }
    }
}
