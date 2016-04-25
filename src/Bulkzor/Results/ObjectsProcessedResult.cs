using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class ObjectsProcessedResult
    {
        public ObjectsProcessedResult(int objectsProcessed, int objectsNotProcessed, int objectsNotProcessedStored)
        {
            ObjectsProcessed = objectsProcessed;
            ObjectsNotProcessed = objectsNotProcessed;
            ObjectsNotProcessedStored = objectsNotProcessedStored;
        }

        public int ObjectsProcessed { get; }
        public int ObjectsNotProcessed { get; }
        public int ObjectsNotProcessedStored { get; }
    }
}