using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Speedygeek.ZendeskAPI.UnitTests.Configuration
{
    [TestFixture]
    public class ServiceCollectionExtensionsTest
    {

        [Test]
        public void BasicAuthSubDomainNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClientWithBasicAuth(null, Settings.AdminUserName, Settings.AdminPassword); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: subDomain"));
        }

        [Test]
        public void BasicAuthUserNameNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClientWithBasicAuth(Settings.SubDomain, null, Settings.AdminPassword); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: userName"));
        }

        [Test]
        public void BasicAuthPasswordNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClientWithBasicAuth(Settings.SubDomain, Settings.AdminUserName, null); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: password"));
        }

        [Test]
        public void ApiTokenAuthTokenNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClientWithApiTokenAuth(Settings.SubDomain, Settings.AdminUserName, null); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: apiToken"));
        }

        [Test]
        public void ApiTokenAuthUserNameNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClientWithApiTokenAuth(Settings.SubDomain, null, Settings.AdminPassword); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: userName"));
        }

        [Test]
        public void ApiTokenAuthSubDomainNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClientWithApiTokenAuth(null, Settings.AdminPassword, Settings.AdminPassword); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: subDomain"));
        }
    }
}
