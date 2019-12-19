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
        public void SubDomainNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClient(null, Settings.AdminUserName, Settings.AdminPassword); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: subDomain"));
        }

        [Test]
        public void UserNameNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClient(Settings.SubDomain, null, Settings.AdminPassword); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: userName"));
        }

        [Test]
        public void PasswordNull()
        {
            var collection = new ServiceCollection();
            Assert.That(() => { collection.AddZendeskClient(Settings.SubDomain, Settings.AdminUserName, null); },
                 Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null.\r\nParameter name: password"));
        }
    }
}
