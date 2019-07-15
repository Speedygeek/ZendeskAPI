using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Speedygeek.ZendeskAPI.IntegrationTests
{
    public class BaseTests
    {
        protected IZendeskClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            var collection = new ServiceCollection();
            collection.AddZendeskClient(Settings.SubDomain, Settings.AdminUserName, Settings.AdminPassword);
            var serviceProvider = collection.BuildServiceProvider();
            _client = serviceProvider.GetService<IZendeskClient>();
        }
    }
}
