using NUnit.Framework;

namespace Speedygeek.ZendeskAPI.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            new ZendeskClient("Test");
            Assert.Pass();
        }
    }
}
