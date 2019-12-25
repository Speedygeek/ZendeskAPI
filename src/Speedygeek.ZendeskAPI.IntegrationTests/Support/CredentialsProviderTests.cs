using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speedygeek.ZendeskAPI.IntegrationTests.Support
{
    [TestFixture]
    public class CredentialsProviderTests : BaseTests
    {
        [Test]
        public async Task OAuthCredTestAsync()
        {
            var collection = new ServiceCollection();
            collection.AddZendeskClientWithOAuthTokenAuth(Settings.SubDomain, Settings.AdminOAuthToken);
            var serviceProvider = collection.BuildServiceProvider();

            Client = serviceProvider.GetService<IZendeskClient>();
            var resp = await Client.Support.Tickets.GetAll().ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }
    }
}
