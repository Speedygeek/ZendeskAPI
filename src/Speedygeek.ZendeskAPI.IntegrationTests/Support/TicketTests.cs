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

            var resp1 = await Client.Support.Tickets.CreateAsync(newTicket).ConfigureAwait(false);

            var id = resp1.Ticket.Id;
            var resp = await Client.Support.Tickets.GetAsync(id).ConfigureAwait(false);
            var ticket = resp.Ticket;

            Assert.That(resp, Is.Not.Null);
            Assert.That(ticket.Id, Is.EqualTo(id));

            ticket.Subject = "this is an updated ticket";
            ticket.RawSubject = null;
            ticket.Comment = new Comment { Body = "Updated subject", Public = false };
            var updateResp = await Client.Support.Tickets.UpdateAsync(ticket).ConfigureAwait(false);

            Assert.That(updateResp.Ticket.Subject, Is.EqualTo(ticket.Subject));

            var deleteResp = await Client.Support.Tickets.DeleteAsync(id).ConfigureAwait(false);

            Assert.That(deleteResp, Is.True);

        }

        [Test]
        public async Task TicketsByOrgId()
        {
            var resp = await Client.Support.Tickets.GetByOrganizationAsync(22560572).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }
    }
}
