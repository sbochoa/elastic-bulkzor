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

        private DataProcessor<T> GetDataProcessor<T>(IProcessChunks<T> chunksProcessor)
            where T : class, IIndexableObject
        {
            return new DataProcessor<T>(chunksProcessor, LogManager.GetLogger("MyLogger"));
        }

        [Test]
        [TestCase(10, 5)]
        [TestCase(10, 3)]
        [TestCase(10, 10)]
        public void IndexData_WhenChunkProcessorWorksCorrectly_ShouldReturnZeroObjectsNotProcessed(int objectsQuantity, int chunkSize)
        {
            var data = new FakeSource(objectsQuantity).GetData<Person>().ToList();

            var dataProcessor = GetDataProcessor(new FakeIProcessChunks<Person>(chunks => new ObjectsProcessedResult(chunks.Sum(c => c.Data.Count), 0, 0)));

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

            var dataProcessor = GetDataProcessor(new FakeIProcessChunks<Person>(chunks => new ObjectsProcessedResult
                                                            (chunks.Sum(c => c.Data.Count) - numberOfObjectsNotProcessed, numberOfObjectsNotProcessed, 0)));

            var result = dataProcessor.IndexData(data, _indexNameFunc, _typeName, chunkSize);

            result.ObjectsProcessed.ShouldBe(objectsQuantity - (numberOfObjectsNotProcessed * numberOfChunks));
            result.ObjectsNotProcessed.ShouldBe(numberOfObjectsNotProcessed * numberOfChunks);
        }
    }
}
