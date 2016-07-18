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

        internal ObjectsProcessedResult()
            :this(0, 0, 0)
        {
            
        }

        public static ObjectsProcessedResult Default { get; } = new ObjectsProcessedResult(0, 0, 0);
        public int ObjectsProcessed { get; private set; }
        public int ObjectsNotProcessed { get; private set; }
        public int ObjectsNotProcessedStored { get; private set; }

        public void Add(ObjectsProcessedResult result)
        {
            ObjectsProcessed += result.ObjectsProcessed;
            ObjectsNotProcessed += result.ObjectsNotProcessed;
            ObjectsNotProcessedStored += result.ObjectsNotProcessedStored;
        }
    }
}