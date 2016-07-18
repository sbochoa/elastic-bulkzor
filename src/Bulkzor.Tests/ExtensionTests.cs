using Bulkzor.Utilities;
using NUnit.Framework;
using Shouldly;

namespace Bulkzor.Tests
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void GetIdFromUnknowObject_WithAnonymousObject_ShouldReturnId()
        {
            object sut = new {Id = "1"};
            var id = sut.GetIdFromUnknowObject();

            id.ShouldBe("1");
        }
    }
}
