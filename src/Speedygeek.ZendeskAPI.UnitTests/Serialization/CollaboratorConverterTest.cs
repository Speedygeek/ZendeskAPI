using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Serialization;


namespace Speedygeek.ZendeskAPI.UnitTests
{
    [TestFixture]
    public class CollaboratorConverterTest
    {
        private ISerializer _serializer;

        [OneTimeSetUp]
        public void SetUp()
        {
            var collection = new ServiceCollection();
            collection.AddZendeskClientWithApiTokenAuth("thisIsAtest", "test@gmail.com", "Mock_ApiToken");
            var serviceProvider = collection.BuildServiceProvider();
            _serializer = serviceProvider.GetRequiredService<ISerializer>();
        }

        [Test]
        public void ConvertMixedTypes()
        {
            var json = @"{ ""Ticket"": { ""id"": 1002, ""collaborators"": [ 562562562, ""someone@example.com"", { ""name"": ""Someone Else"", ""email"": ""else@example.com"" } ]}}";

            TicketResponse resp = null;
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(json)))
            {
                resp = _serializer.Deserialize<TicketResponse>(stream);
            }

            Assert.That(resp.Ticket, Is.Not.Null);
            Assert.That(resp.Ticket.Collaborators[0].Id, Is.Not.Zero);
            Assert.That(resp.Ticket.Collaborators[1].Email, Is.EqualTo("someone@example.com"));

            Assert.That(resp.Ticket.Collaborators[2].Email, Is.EqualTo("else@example.com"));
            Assert.That(resp.Ticket.Collaborators[2].Name, Is.EqualTo("Someone Else"));
        }
    }
}
