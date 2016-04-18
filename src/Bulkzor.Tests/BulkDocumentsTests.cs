//using System;
//using System.Linq;
//using Bulkzor.Commands;
//using Bulkzor.Configuration;
//using Bulkzor.Handlers;
//using Bulkzor.Results;
//using Bulkzor.Tests.Fakes;
//using Moq;
//using NUnit.Framework;

//namespace Bulkzor.Tests
//{
//    [TestFixture]
//    public class BulkDocumentsTests
//        : BaseTest
//    {
//        [Test]
//        public void Handle_WithChunkSize25_()
//        {
//            var chunkConfiguration = new ChunkConfiguration().ChunkSize(25);
//            var command = CreateCommand(50, chunkConfiguration);

//            var indexChunkHandler = new Mock<IHandle<IndexChunk.Command<Document>, IndexResult>>();

//            indexChunkHandler.Setup(m => m.Handle(null)).Returns(new IndexResult(20, 5, TimeSpan.Zero));

//            var handler = CreateHandler();

//            //var result = handler.Handle(command);

//            //result.ObjectsIndexed.ShouldBe(fakeDocuments.Count());
//        }

//        private BulkDocuments.Command<Document> CreateCommand(int documentsQuantity, ChunkConfiguration chunkConfiguration)
//        {
//            var fakeDocuments = GetFakeDocumentsList(documentsQuantity).ToList();
//            return new BulkDocuments.Command<Document>(fakeDocuments, _indexName, _typeName, chunkConfiguration);
//        }

//        private BulkDocuments.Handler<Document> CreateHandler()
//        {
//            return new BulkDocuments.Handler<Document>(IndexChunkHandlerFake.Instance);
//        } 
//    }
//}
