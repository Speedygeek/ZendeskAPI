using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Speedygeek.ZendeskAPI.IntegrationTests
{
    public class BaseTests
    {
        protected IZendeskClient Client { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            var collection = new ServiceCollection();
            collection.AddZendeskClient(Settings.SubDomain, Settings.AdminUserName, Settings.AdminPassword);

            var serviceProvider = collection.BuildServiceProvider();
            Client = serviceProvider.GetService<IZendeskClient>();
        }
    }
}
