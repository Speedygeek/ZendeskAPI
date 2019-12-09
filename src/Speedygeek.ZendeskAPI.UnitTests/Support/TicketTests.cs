using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Operations.Support;
using Speedygeek.ZendeskAPI.UnitTests.Base;

namespace Speedygeek.ZendeskAPI.UnitTests.Support
{
    [TestFixture]
    public class TicketTests : TestBase
    {

        [Test]
        public async Task TicketCreateReadUpdateDelete()
        {
            BuildResponse("/api/v2/tickets.json", "createTicket.json", HttpMethod.Post);

            var newTicket = new Ticket()
            {
                Subject = "my printer is on fire",
                Comment = new Comment { Body = "HELP" },
                Priority = TicketPriority.Urgent,
            };

            var createResp = await Client.Support.Tickets.Create(newTicket).ConfigureAwait(false);

            Assert.That(createResp, Is.Not.Null);
            Assert.That(createResp.Ticket.Id, Is.EqualTo(24116));

            BuildResponse("/api/v2/tickets/24116.json", "createTicket.json");

            var id = createResp.Ticket.Id;
            var getResp = await Client.Support.Tickets.Get(id).ConfigureAwait(false);

            Assert.That(getResp, Is.Not.Null);
            Assert.That(getResp.Ticket.Id, Is.EqualTo(id));

            BuildResponse("/api/v2/tickets/24116.json", "update_24116_Ticket.json", HttpMethod.Post);
            var ticket = getResp.Ticket;
            ticket.Subject = "this is an updated ticket";
            ticket.Comment = new Comment { Body = "Updated subject", Public = false };
            var updateResp = await Client.Support.Tickets.Update(ticket).ConfigureAwait(false);

            Assert.That(updateResp.Ticket.Subject, Is.EqualTo(ticket.Subject));

            BuildResponse("/api/v2/tickets/24116.json", string.Empty, HttpMethod.Delete);

            var deleteResp = await Client.Support.Tickets.Delete(id).ConfigureAwait(false);

            Assert.That(deleteResp, Is.True);
        }

        [Test]
        public async Task TicketsByOrg()
        {
            BuildResponse("/api/v2/organizations/22560572/tickets.json?page=1&per_page=50", "organization_22560572_tickets.json");

            var resp = await Client.Support.Tickets.GetByOrganization(22560572, new TicketPageParams { PerPage = 50 }, TicketSideloads.None).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(50));
            Assert.That(resp.NextPage, Is.Not.Null);
            Assert.That(resp.PerPage, Is.EqualTo(50));
            Assert.That(resp.TotalPages, Is.EqualTo(27));
        }

        [Test]
        public async Task TicketsGetAll()
        {
            BuildResponse("/api/v2/tickets.json", "tickets.json");

            var resp = await Client.Support.Tickets.GetAll().ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }

        [Test]
        public async Task TicketsGetAllWithSideLoad()
        {
            BuildResponse("/api/v2/tickets.json?include=users,comment_count", "ticketsWithSideload.json");

            var resp = await Client.Support.Tickets.GetAll(sideload: TicketSideloads.Users | TicketSideloads.Comment_Count).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
            Assert.That(resp.Tickets.Any(t => t.CommentCount > 0));
        }
    }
}
