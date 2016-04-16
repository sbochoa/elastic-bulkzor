using Bulkzor.Callbacks;

namespace Bulkzor.Configuration
{
    public class ChunkConfiguration
    {
        private int? _chunkSize;
        private int? _chunkRetries;
        private OnChunkIndexed _onChunkIndexed;
        private bool? _partChunkWhenFail;
        private int? _numberPartsToDivideWhenChunkFail;

        internal int GetChunkSize => _chunkSize ?? 1000;
        internal int GetChunkRetries => _chunkRetries ?? 5;
        internal OnChunkIndexed GetOnChunkIndexed => _onChunkIndexed;
        internal bool GetPartChunkWhenFail => _partChunkWhenFail ?? true;
        internal int GetNumberPartsToDivideWhenChunkFail => _numberPartsToDivideWhenChunkFail ?? 5;

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

        public void PartChunkWhenFail(bool partChunkWhenFail)
        {
            _partChunkWhenFail = partChunkWhenFail;
        }

        public void NumberPartsToDivideWhenChunkFail(int numberPartsToDivideWhenChunkFail)
        {
            _numberPartsToDivideWhenChunkFail = numberPartsToDivideWhenChunkFail;
        }
    }
}
