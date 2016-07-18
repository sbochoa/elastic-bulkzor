using Bulkzor.Tests.Fakes;
using Common.Logging;

namespace Bulkzor.Tests.Processors
{
    public class BaseProcessorTests
    {
        protected FakeObjectsIndexer FakeObjectsIndexer => new FakeObjectsIndexer();
        protected static readonly ILog Logger = LogManager.GetLogger("MyLogger");
    }
}
