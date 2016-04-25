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
        private readonly Func<Person, string> _indexNameFunc = person => "IndexName";
        private readonly string _typeName = "TypeName";
        private static List<Person> _emptyPersonList = new List<Person>();
        private ChunkProcessor<T> GetChunkProcessor<T>(IIndexObjects<T> objectsIndexer, IHandleIndexErrors<T> indexErrorsHandler)
            where T : class, IIndexableObject
        {
            return new ChunkProcessor<T>(objectsIndexer, indexErrorsHandler, LogManager.GetLogger("MyLogger"));
        }

        [Test]
        [TestCase(10)]
        [TestCase(15)]
        public void ProcessChunks_WhenObjectsIndexerWorksCorrectly_ShouldReturnCorrectResult(int objectsQuantity)
        {
            var chunkProcessor = GetChunkProcessor
                (new FakeIIndexObjects<Person>(objects => new IndexObjectsResult<Person>(objects, _emptyPersonList, IndexingError.None)), null);

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
                (new FakeIIndexObjects<Person>(objects => new IndexObjectsResult<Person>(_emptyPersonList, objects, IndexingError.LengthExceeded))
                , new FakeIndexErrorsHandler<Person>((error, objectsNotIndexed) => new IndexObjectsResult<Person>(objectsNotIndexed, _emptyPersonList, IndexingError.None)));

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
                (new FakeIIndexObjects<Person>(objects => new IndexObjectsResult<Person>(_emptyPersonList, objects, IndexingError.Unknow))
                , new FakeIndexErrorsHandler<Person>
                    ((error, objectsNotIndexed) 
                        => new IndexObjectsResult<Person>(_emptyPersonList, objectsNotIndexed, IndexingError.OnlyPartOfDocumentsIndexed, objectsNotIndexed)));

            var chunks = GetChunks(objectsQuantity);
            var processChunksResult = chunkProcessor.ProcessChunks(chunks);

            processChunksResult.ObjectsProcessed.ShouldBe(0);
            processChunksResult.ObjectsNotProcessed.ShouldBe(objectsQuantity);
            processChunksResult.ObjectsNotProcessedStored.ShouldBe(objectsQuantity);
        }

        private IReadOnlyList<Chunk<Person>> GetChunks(int objectsQuantity)
        {
            var chunkStore = new ChunkStore<Person>(_indexNameFunc, _typeName, objectsQuantity);

            var data = new FakeSource(objectsQuantity).GetData<Person>();

            foreach (var person in data)
            {
                chunkStore.AddObjectToChunk(person);
            }

            return chunkStore.Chunks;
        }
    }
}
