using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class ObjectsProcessedResult
    {
        public ObjectsProcessedResult(int objectsProcessed, int objectsNotProcessed)
        {
            ObjectsProcessed = objectsProcessed;
            ObjectsNotProcessed = objectsNotProcessed;
        }

        public int ObjectsProcessed { get; }
        public int ObjectsNotProcessed { get; }
    }
}