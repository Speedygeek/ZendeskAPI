using System.Collections.Generic;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Operations.Support;

namespace Speedygeek.ZendeskAPI.UnitTests.Serialization
{
    [TestFixture]
    public class PageParametersTests
    {
        [Test]
        public void PageParametersDefaultParams()
        {
            var ticketParams = new TicketPageParams();

            var parameters = ticketParams.ToParameters();

            Assert.That(parameters, Contains.Item(new KeyValuePair<string, string>("page", "1")));
            Assert.That(parameters, Contains.Item(new KeyValuePair<string, string>("per_page", "100")));
            Assert.That(parameters, Does.Not.ContainKey("sort_order"));
            Assert.That(parameters, Does.Not.ContainKey("sort_by"));
        }

        [Test]
        public void PageParametersWithSortOrder()
        {
            var ticketParams = new TicketPageParams { SortBy = TicketSortBy.Id, SortOrder = SortOrder.Descending };

            var parameters = ticketParams.ToParameters();

            Assert.That(parameters, Contains.Item(new KeyValuePair<string, string>("page", "1")));
            Assert.That(parameters, Contains.Item(new KeyValuePair<string, string>("per_page", "100")));
            Assert.That(parameters, Contains.Item(new KeyValuePair<string, string>("sort_order", "desc")));
            Assert.That(parameters, Contains.Item(new KeyValuePair<string, string>("sort_by", "id")));
        }
    }
}
