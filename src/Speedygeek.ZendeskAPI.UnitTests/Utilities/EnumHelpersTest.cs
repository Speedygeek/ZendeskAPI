using NUnit.Framework;
using Speedygeek.ZendeskAPI.Operations.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.UnitTests.Utilities
{
    [TestFixture]
    public class EnumHelpersTest
    {
        [Test]
        public void GetDisplayNameTest()
        {
            var name = SortOrder.Ascending.GetDisplayName();

            Assert.That(name, Is.EqualTo("asc"));

            name = SortOrder.None.GetDisplayName();

            Assert.That(name, Is.EqualTo("none"));
        }
    }
}
