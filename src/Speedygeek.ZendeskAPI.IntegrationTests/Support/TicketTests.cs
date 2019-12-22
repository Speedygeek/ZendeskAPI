using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.IntegrationTests.Support
{
    public class TicketTests : BaseTests
    {
        [Test]
        public async Task TicketCreateReadUpdateDelete()
        {
            var newTicket = new Ticket()
            {
                Subject = "my printer is on fire",
                Comment = new Comment { Body = "HELP" },
                Priority = TicketPriority.Urgent,
            };

            var resp1 = await Client.Support.Tickets.Create(newTicket).ConfigureAwait(false);

            var id = resp1.Ticket.Id;
            var resp = await Client.Support.Tickets.Get(id).ConfigureAwait(false);

            Assert.That(resp, Is.Not.Null);
            Assert.That(resp.Ticket.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task TicketsByOrgId()
        {
            var resp = await Client.Support.Tickets.GetByOrganization(22560572).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }
    }
}
