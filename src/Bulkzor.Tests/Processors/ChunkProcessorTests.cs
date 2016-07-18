using System;
using System.Collections.Generic;
using Bulkzor.Configuration;
using Bulkzor.Indexers;
using Bulkzor.Models;
using Bulkzor.Processors;
using Bulkzor.Tests.Fakes;
using Bulkzor.Types;
using NUnit.Framework;
using Shouldly;

namespace Bulkzor.Tests.Processors
{
    [TestFixture]
    public class ChunkProcessorTests : BaseProcessorTests
    {
        private readonly Func<object, string> _indexNameFunc = person => "WithIndexName";
        private readonly string _typeName = "WithTypeName";
        private static readonly List<Person> EmptyPersonList = new List<Person>();

        private ChunkProcessor GetChunkProcessor(IObjectIndexer objectsIndexer)
        {
            return new ChunkProcessor(new ChunkConfiguration(), objectsIndexer, new FakeObjectsStore(), Logger);
        }

        private ChunkProcessor GetChunkProcessor()
        {
            return GetChunkProcessor(FakeObjectsIndexer);
        }

        [Test]
        [TestCase(10)]
        [TestCase(15)]
        public void ProcessChunks_WhenObjectsIndexerWorksCorrectly_ShouldReturnCorrectResult(int objectsQuantity)
        {
            var chunkProcessor = GetChunkProcessor();

            var chunk = GetChunk(objectsQuantity);
            var processChunksResult = chunkProcessor.ProcessChunk(chunk);

            processChunksResult.ObjectsProcessed.ShouldBe(objectsQuantity);
            processChunksResult.ObjectsNotProcessed.ShouldBe(0);
        }

        [Test]
        [TestCase(10)]
        [TestCase(15)]
        public void ProcessChunks_WhenObjectsIndexerReturnsLengthExceeded_ShouldReturnCorrectResult(int objectsQuantity)
        {
            var chunk = GetChunk(objectsQuantity);

            var chunkProcessor = GetChunkProcessor(new FakeObjectsIndexer { IndexingError = IndexingError.LengthExceeded, ObjectsNotIndexed = chunk });

            var processChunksResult = chunkProcessor.ProcessChunk(chunk);

            processChunksResult.ObjectsProcessed.ShouldBe(0);
            processChunksResult.ObjectsNotProcessed.ShouldBe(objectsQuantity);
        }

        [Test]
        [TestCase(10)]
        [TestCase(15)]
        public void ProcessChunks_WhenObjectsIndexerReturnsUnknowError_ShouldReturnCorrectResult(int objectsQuantity)
        {
            var chunk = GetChunk(objectsQuantity);

            var chunkProcessor = GetChunkProcessor(new FakeObjectsIndexer { IndexingError = IndexingError.Unknow, ObjectsNotIndexed = chunk });

            var processChunksResult = chunkProcessor.ProcessChunk(chunk);

            processChunksResult.ObjectsProcessed.ShouldBe(0);
            processChunksResult.ObjectsNotProcessed.ShouldBe(objectsQuantity);
        }

        private Chunk GetChunk(int objectsQuantity)
        {
            var data = new FakeSource(objectsQuantity).GetData();

            var chunk = new Chunk(_indexNameFunc(null), _typeName);

            chunk.AddRange(data);

            return chunk;
        }
    }
}
