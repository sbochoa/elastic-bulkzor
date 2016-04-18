namespace Bulkzor.Results
{
    public class IndexDataChunkResult
    {
        public IndexDataChunkResult(int objectsIndexed, int objectsNotIndexed)
        {
            ObjectsIndexed = objectsIndexed;
            ObjectsNotIndexed = objectsNotIndexed;
        }

        public int ObjectsIndexed { get; }
        public int ObjectsNotIndexed { get; }
    }
}
