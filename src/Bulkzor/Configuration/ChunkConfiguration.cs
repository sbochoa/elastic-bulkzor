using Bulkzor.Callbacks;

namespace Bulkzor.Configuration
{
    public class ChunkConfiguration
    {
        public int ChunkSize { get; set; } = 1000;
        public int ChunkRetries { get; set; } = 5;
        private bool PartChunkWhenFail { get; set; } = true;
    }
}
