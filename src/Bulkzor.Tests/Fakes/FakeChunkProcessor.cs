//using System.Collections.Generic;
//using System.Linq;
//using Bulkzor.Models;
//using Bulkzor.Processors;
//using Bulkzor.Results;

//namespace Bulkzor.Tests.Fakes
//{
//    public class FakeChunkProcessor : IProcessChunks
//    {
//        public FakeChunkProcessor()
//        {

//        }

//        public int ObjectsProcessed { get; private set; }
//        public int ObjectsNotProcessed { get; private set; }
//        public int NumberOfObjectsToFailPerProcessChunks { get; set; }
//        public ObjectsProcessedResult ProcessChunks(IReadOnlyList<Chunk> chunks)
//        {
//            var objects = chunks.Sum(c => c.Data.Count);
//            ObjectsProcessed += objects - NumberOfObjectsToFailPerProcessChunks;
//            ObjectsNotProcessed += NumberOfObjectsToFailPerProcessChunks;
//             return new ObjectsProcessedResult(objects - NumberOfObjectsToFailPerProcessChunks, NumberOfObjectsToFailPerProcessChunks, NumberOfObjectsToFailPerProcessChunks);
//        }
//    }
//}
