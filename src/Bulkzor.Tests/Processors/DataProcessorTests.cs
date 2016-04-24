using System;
using System.Linq;
using Bulkzor.Processors;
using Bulkzor.Results;
using Bulkzor.Tests.Fakes;
using Common.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;


namespace Bulkzor.Tests.Processors
{
    [TestFixture]
    public class DataProcessorTests
    {
        private readonly Func<Person, string> _indexNameFunc = person => "IndexName";
        private readonly string _typeName = "TypeName";

        private DataProcessor GetDataProcessor(Action<Mock<IProcessChunks>> mockConfiguration)
        {
            var chunkProcessorMock = new Mock<IProcessChunks>();
            mockConfiguration(chunkProcessorMock);
            return new DataProcessor(chunkProcessorMock.Object, LogManager.GetLogger("MyLogger"));
        }

        private DataProcessor GetDataProcessor(IProcessChunks chunksProcessor)
        {
            return new DataProcessor(chunksProcessor, LogManager.GetLogger("MyLogger"));
        }

        [Test]
        [TestCase(10, 5)]
        [TestCase(10, 3)]
        [TestCase(10, 10)]
        public void IndexData_WhenChunkProcessorWorksCorrectly_ShouldReturnZeroObjectsNotProcessed(int objectsQuantity, int chunkSize)
        {
            var data = new FakeSource(objectsQuantity).GetData<Person>().ToList();

            var dataProcessor = GetDataProcessor(new FakeIProcessChunks(realResult => realResult));

            var result = dataProcessor.IndexData(data, _indexNameFunc, _typeName, chunkSize);

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
            var data = new FakeSource(objectsQuantity).GetData<Person>().ToList();
            var numberOfChunks = (int)Math.Ceiling((double)objectsQuantity/chunkSize);

            var dataProcessor = GetDataProcessor(new FakeIProcessChunks(realResult => new ObjectsProcessedResult
                                                            (realResult.ObjectsProcessed - numberOfObjectsNotProcessed, numberOfObjectsNotProcessed)));

            var result = dataProcessor.IndexData(data, _indexNameFunc, _typeName, chunkSize);

            result.ObjectsProcessed.ShouldBe(objectsQuantity - (numberOfObjectsNotProcessed * numberOfChunks));
            result.ObjectsNotProcessed.ShouldBe(numberOfObjectsNotProcessed * numberOfChunks);
        }
    }
}
