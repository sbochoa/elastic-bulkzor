using Bulkzor.Callbacks;

namespace Bulkzor.Configuration
{
    public class ChunkConfiguration
    {
        private int _chunkSize;
        private int _chunkRetries;
        private OnChunkIndexed _onChunkIndexed;
        private bool _partChunkWhenFail;
        private int _numberPartsToDivideWhenChunkFail;

        internal int GetChunkSize => _chunkSize == 0 ? 500 : _chunkSize;
        internal int GetChunkRetries => _chunkRetries == 0 ? 5 : _chunkRetries;
        internal OnChunkIndexed GetOnChunkIndexed => _onChunkIndexed;
        internal bool GetPartChunkWhenFail => _partChunkWhenFail;
        internal int GetNumberPartsToDivideWhenChunkFail
            => _numberPartsToDivideWhenChunkFail == 0 ? 5 : _numberPartsToDivideWhenChunkFail;

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

        public ChunkConfiguration OnBulkIndexed(OnChunkIndexed onChunkIndexed)
        {
            _onChunkIndexed = onChunkIndexed;
            return this;
        }

        public void PartChunkWhenFail(bool partChunkWhenFail = true)
        {
            _partChunkWhenFail = partChunkWhenFail;
        }

        public void NumberPartsToDivideWhenChunkFail(int numberPartsToDivideWhenChunkFail)
        {
            _numberPartsToDivideWhenChunkFail = numberPartsToDivideWhenChunkFail;
        }
    }
}
