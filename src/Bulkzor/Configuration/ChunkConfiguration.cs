using Bulkzor.Callbacks;

namespace Bulkzor.Configuration
{
    public class ChunkConfiguration
    {
        private int? _chunkSize;
        private int? _chunkRetries;
        private BeforeIndexDataChunk _beforeIndexDataChunk;
        private bool? _partChunkWhenFail;
        private AfterIndexDataChunk _afterIndexDataChunk;

        internal int GetChunkSize => _chunkSize ?? 1000;
        internal int GetChunkRetries => _chunkRetries ?? 5;
        internal BeforeIndexDataChunk GetBeforeIndexDataChunk => _beforeIndexDataChunk;
        internal AfterIndexDataChunk GetAfterIndexDataChunk => _afterIndexDataChunk;
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

        public ChunkConfiguration BeforeIndexDataChunk(BeforeIndexDataChunk beforeIndexDataChunk)
        {
            _beforeIndexDataChunk = beforeIndexDataChunk;
            return this;
        }

        public ChunkConfiguration AfterIndexDataChunk(AfterIndexDataChunk afterIndexDataChunk)
        {
            _afterIndexDataChunk = afterIndexDataChunk;
            return this;
        }

        public void PartChunkWhenFail(bool partChunkWhenFail)
        {
            _partChunkWhenFail = partChunkWhenFail;
        }
    }
}
