﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
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
            var resp = await Client.Support.Tickets.GetAllAsync().ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }
    }
}
