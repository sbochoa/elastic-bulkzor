using System;
using System.Collections.Generic;
using Bulkzor.Errors;
using Bulkzor.Indexers;
using Bulkzor.Models;
using Bulkzor.Processors;
using Bulkzor.Results;
using Bulkzor.Tests.Fakes;
using Bulkzor.Types;
using Common.Logging;
using NUnit.Framework;
using Shouldly;

namespace Bulkzor.Tests.Processors
{
    [TestFixture]
    public class ChunkProcessorTests
    {
        private readonly Func<object, string> _indexNameFunc = person => "IndexName";
        private readonly string _typeName = "TypeName";
        private static readonly List<Person> EmptyPersonList = new List<Person>();
        private ChunkProcessor GetChunkProcessor(IIndexObjects objectsIndexer, IHandleIndexErrors indexErrorsHandler)
        {
            return new ChunkProcessor(objectsIndexer, indexErrorsHandler, LogManager.GetLogger("MyLogger"));
        }

        [Test]
        [TestCase(10)]
        [TestCase(15)]
        public void ProcessChunks_WhenObjectsIndexerWorksCorrectly_ShouldReturnCorrectResult(int objectsQuantity)
        {
            var chunkProcessor = GetChunkProcessor
                (new FakeIIndexObjects(objects => new IndexObjectsResult(objects, EmptyPersonList, IndexingError.None)), null);

            var chunks = GetChunks(objectsQuantity);
            var processChunksResult = chunkProcessor.ProcessChunks(chunks);

            processChunksResult.ObjectsProcessed.ShouldBe(objectsQuantity);
            processChunksResult.ObjectsNotProcessed.ShouldBe(0);
        }

        [Test]
        [TestCase(10)]
        [TestCase(15)]
        public void ProcessChunks_WhenObjectsIndexerReturnsLengthExceeded_And_WhenIndexErrorsHandlesErrorCorrectly_ShouldReturnCorrectResult(int objectsQuantity)
        {
            var chunkProcessor = GetChunkProcessor
                (new FakeIIndexObjects(objects => new IndexObjectsResult(EmptyPersonList, objects, IndexingError.LengthExceeded))
                , new FakeIndexErrorsHandler((error, objectsNotIndexed) => new IndexObjectsResult(objectsNotIndexed, EmptyPersonList, IndexingError.None)));

            var chunks = GetChunks(objectsQuantity);
            var processChunksResult = chunkProcessor.ProcessChunks(chunks);

            processChunksResult.ObjectsProcessed.ShouldBe(objectsQuantity);
            processChunksResult.ObjectsNotProcessed.ShouldBe(0);
        }

        [Test]
        [TestCase(10)]
        [TestCase(15)]
        public void ProcessChunks_WhenObjectsIndexerReturnsUnknowError_And_WhenIndexErrorsHandlesErrorCorrectly_ShouldReturnCorrectResult(int objectsQuantity)
        {
            var chunkProcessor = GetChunkProcessor
                (new FakeIIndexObjects(objects => new IndexObjectsResult(EmptyPersonList, objects, IndexingError.Unknow))
                , new FakeIndexErrorsHandler
                    ((error, objectsNotIndexed) 
                        => new IndexObjectsResult(EmptyPersonList, objectsNotIndexed, IndexingError.OnlyPartOfDocumentsIndexed, objectsNotIndexed)));

            var chunks = GetChunks(objectsQuantity);
            var processChunksResult = chunkProcessor.ProcessChunks(chunks);

            processChunksResult.ObjectsProcessed.ShouldBe(0);
            processChunksResult.ObjectsNotProcessed.ShouldBe(objectsQuantity);
            processChunksResult.ObjectsNotProcessedStored.ShouldBe(objectsQuantity);
        }

        private IReadOnlyList<Chunk> GetChunks(int objectsQuantity)
        {
            var chunkStore = new ChunkStore(_indexNameFunc, _typeName, objectsQuantity);

            var data = new FakeSource(objectsQuantity).GetData();

            foreach (var person in data)
            {
                chunkStore.AddObjectToChunk(person);
            }

            return chunkStore.Chunks;
        }
    }
}
