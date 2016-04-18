//using System;
//using Bulkzor.Commands;
//using Bulkzor.Handlers;
//using Bulkzor.Results;

//namespace Bulkzor.Tests.Fakes
//{
//    public class IndexChunkHandlerFake
//        : IHandle<IndexChunk.Command<Document>, IndexResult>
//    {
//        public static IndexChunkHandlerFake Instance = new IndexChunkHandlerFake();

//        public IndexResult Handle(IndexChunk.Command<Document> message)
//        {
//            return new IndexResult(message.Chunk.Count, 0, TimeSpan.Zero);
//        }
//    }
//}
