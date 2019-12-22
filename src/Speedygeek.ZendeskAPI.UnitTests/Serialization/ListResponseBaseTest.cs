using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Serialization;

namespace Speedygeek.ZendeskAPI.UnitTests.Serialization
{
    [TestFixture]
    public class ListResponseBaseTest
    {
        private ISerializer _serializer;

        [OneTimeSetUp]
        public void SetUp()
        {
            var collection = new ServiceCollection();
            collection.AddZendeskClient("thisIsAtest", "test@gmail.com", "MOCKPassword");
            var serviceProvider = collection.BuildServiceProvider();
            _serializer = serviceProvider.GetRequiredService<ISerializer>();
        }


        [Test]
        public void PageNumber()
        {
            var json = @"{ ""next_page"":""https://csharpapi.zendesk.com/api/v2/tickets.json?page=3"",""previous_page"":""https://csharpapi.zendesk.com/api/v2/tickets.json?page=1"",""count"":1365}";

            TicketListResponse resp = null;
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(json)))
            {
                resp = _serializer.Deserialize<TicketListResponse>(stream);
            }

            Assert.That(resp.Page, Is.EqualTo(2));
        }


    }
}
