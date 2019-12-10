using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Operations.Support;
using Speedygeek.ZendeskAPI.UnitTests.Base;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.UnitTests.Support
{
    [TestFixture]
    public class TicketTests : TestBase
    {

        [Test]
        public async Task TicketCreateReadUpdateDelete()
        {
            BuildResponse("tickets.json", "createTicket.json", HttpMethod.Post);

            var newTicket = new Ticket()
            {
                Subject = "my printer is on fire",
                Comment = new Comment { Body = "HELP" },
                Priority = TicketPriority.Urgent,
            };

            var createResp = await Client.Support.Tickets.Create(newTicket).ConfigureAwait(false);

            Assert.That(createResp, Is.Not.Null);
            Assert.That(createResp.Ticket.Id, Is.EqualTo(24116));

            BuildResponse("tickets/24116.json", "createTicket.json");

            var id = createResp.Ticket.Id;
            var getResp = await Client.Support.Tickets.Get(id).ConfigureAwait(false);

            Assert.That(getResp, Is.Not.Null);
            Assert.That(getResp.Ticket.Id, Is.EqualTo(id));

            BuildResponse("tickets/24116.json", "update_24116_Ticket.json", HttpMethod.Post);
            var ticket = getResp.Ticket;
            ticket.Subject = "this is an updated ticket";
            ticket.Comment = new Comment { Body = "Updated subject", Public = false };
            var updateResp = await Client.Support.Tickets.Update(ticket).ConfigureAwait(false);

            Assert.That(updateResp.Ticket.Subject, Is.EqualTo(ticket.Subject));

            BuildResponse("tickets/24116.json", string.Empty, HttpMethod.Delete);

            var deleteResp = await Client.Support.Tickets.Delete(id).ConfigureAwait(false);

            Assert.That(deleteResp, Is.True);
        }

        [Test]
        public async Task TicketsByOrg()
        {
            BuildResponse("organizations/22560572/tickets.json?page=1&per_page=50", "organization_22560572_tickets.json");

            var resp = await Client.Support.Tickets.GetByOrganization(22560572, new TicketPageParams { PerPage = 50 }, TicketSideloads.None).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(50));
            Assert.That(resp.NextPage, Is.Not.Null);
            Assert.That(resp.PerPage, Is.EqualTo(50));
            Assert.That(resp.TotalPages, Is.EqualTo(27));
        }

        [Test]
        public async Task TicketsGetAll()
        {
            BuildResponse("tickets.json", "tickets.json");

            var resp = await Client.Support.Tickets.GetAll().ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }

        [Test]
        public async Task TicketsGetAllWithSideLoad()
        {
            BuildResponse("tickets.json?include=users,comment_count", "ticketsWithSideload.json");

            var resp = await Client.Support.Tickets.GetAll(sideload: TicketSideloads.Users | TicketSideloads.Comment_Count).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
            Assert.That(resp.Tickets.Any(t => t.CommentCount > 0));
        }

        [Test]
        public async Task TicketsDeleteBulk()
        {
            var ids = new List<long> { 896, 902, 903 };

            BuildResponse($"tickets/destroy_many.json?ids={ids.ToCsv()}", "DeleteBulk.json", HttpMethod.Delete);

            var resp = await Client.Support.Tickets.DeleteBulk(ids).ConfigureAwait(false);

            Assert.That(resp.JobStatus.URL.ToString(), Is.EqualTo($"https://csharpapi.zendesk.com/api/v2/job_statuses/{resp.JobStatus.Id}.json"));
        }

        [Test]
        public void TicketsDeleteBulkWithOver100()
        {

            var random = new Random();
            var ids = new List<long> ();
            for (int i = 0; i < 102; i++)
            {
                ids.Add(random.Next());
            }

            Assert.That(async () => { var resp = await Client.Support.Tickets.DeleteBulk(ids).ConfigureAwait(false); },
                Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: ids"));
        }

    }
}
