using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Speedygeek.ZendeskAPI;
using Speedygeek.ZendeskAPI.IntegrationTests;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.IntegrationTests
{
    public class TicketTest
    {
        private IZendeskClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            var collection = new ServiceCollection();
            collection.AddZendeskClient(Settings.SubDomain, Settings.AdminUserName, Settings.AdminPassword);
            var serviceProvider = collection.BuildServiceProvider();
            _client = serviceProvider.GetService<IZendeskClient>();
        }

        [Test]
        public async Task Ticket_CRUD()
        {
            var newTicket = new Ticket()
            {
                Subject = "my printer is on fire",
                // Comment = new Comment { Body = "HELP" },
                // Priority = TicketPriorities.Urgent,
            };

            long id = 1364;
            var resp = await _client.Tickets.GetTicket(id);

            Assert.That(resp, Is.Not.Null);
            Assert.That(resp.Ticket.Id, Is.EqualTo(id));
        }
    }
}
