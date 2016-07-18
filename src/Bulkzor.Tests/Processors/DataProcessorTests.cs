using System;
using System.Linq;
using Bulkzor.Configuration;
using Bulkzor.Processors;
using Bulkzor.Tests.Fakes;
using Bulkzor.Types;
using NUnit.Framework;
using Shouldly;


namespace Bulkzor.Tests.Processors
{
    [TestFixture]
    public class DataProcessorTests : BaseProcessorTests
    {
        private readonly Func<object, string> _indexNameFunc = person => "WithIndexName";
        private readonly string _typeName = "WithTypeName";

        private DataProcessor GetDataProcessor(ChunkProcessor chunksProcessor)
        {
            return new DataProcessor(chunksProcessor, Logger);
        }

        private ChunkProcessor GetChunkProcessor(int chunkSize)
        {
            return new ChunkProcessor(new ChunkConfiguration { ChunkSize = chunkSize }, FakeObjectsIndexer, new FakeObjectsStore(), Logger);
        }
        

        [Test]
        [TestCase(10, 5)]
        [TestCase(10, 3)]
        [TestCase(10, 10)]
        public void IndexData_WhenChunkProcessorWorksCorrectly_ShouldReturnZeroObjectsNotProcessed(int objectsQuantity, int chunkSize)
        {
            var data = new FakeSource(objectsQuantity).GetData().ToList();
            var fakeChunkProcessor = GetChunkProcessor(chunkSize);

            var dataProcessor = GetDataProcessor(fakeChunkProcessor);

            var result = dataProcessor.IndexData(data, _indexNameFunc, _typeName);

            result.ObjectsProcessed.ShouldBe(objectsQuantity);
            result.ObjectsNotProcessed.ShouldBe(0);
        }

        [Test]
        [TestCase(10, 7, 3)]
        [TestCase(10, 5, 3)]
        [TestCase(10, 10, 3)]
        public void IndexData_WhenChunkProcessorDoesNotWorksCorrectly_ShouldReturnCorrectNumberOfObjectsProcessedAndNotProcessed
            (int objectsQuantity, int chunkSize, int numberOfObjectsNotProcessed)
        {
            var data = new FakeSource(objectsQuantity).GetData().ToList();
            var dataNotProcessed = data.Where((x, i) => i < 3).ToList();

            var fakeChunkProcessor = new ChunkProcessor(new ChunkConfiguration { ChunkSize = chunkSize }
                                                    , new FakeObjectsIndexer() { IndexingError = IndexingError.Unknow, ObjectsNotIndexed = dataNotProcessed }
                                                    , new FakeObjectsStore()
                                                    , Logger);

            var dataProcessor = GetDataProcessor(fakeChunkProcessor);

            var result = dataProcessor.IndexData(data, _indexNameFunc, _typeName);

            result.ObjectsProcessed.ShouldBe(objectsQuantity - numberOfObjectsNotProcessed);
            result.ObjectsNotProcessed.ShouldBe(numberOfObjectsNotProcessed);
        }
    }
}
