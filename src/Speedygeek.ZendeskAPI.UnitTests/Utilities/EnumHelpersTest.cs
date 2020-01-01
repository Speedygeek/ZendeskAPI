using NUnit.Framework;
using Speedygeek.ZendeskAPI.Operations.Support;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.UnitTests.Utilities
{
    [TestFixture]
    public class EnumHelpersTest
    {
        [Test]
        public void GetDisplayNameTest()
        {
            var name = SortOrder.Ascending.GetEnumMemberValue();

            Assert.That(name, Is.EqualTo("asc"));

            name = SortOrder.None.GetEnumMemberValue();

            Assert.That(name, Is.EqualTo("none"));
        }
    }
}
