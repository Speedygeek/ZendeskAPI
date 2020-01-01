using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.UnitTests.Base;
using System.Net.Http;
using System.Threading.Tasks;

namespace Speedygeek.ZendeskAPI.UnitTests.Support
{
    [TestFixture]
    public class UserTests : TestBase
    {
        [Test]
        public async Task UserGetInRoles()
        {
            BuildResponse("users.json?role[]=end-user&role[]=admin", "UserGetInRoles.json", HttpMethod.Get);

            var resp = await Client.Support.Users.GetInRoles(UserRoles.EndUser | UserRoles.Admin);

            Assert.That(resp.Users, Has.Some.Property("Role").EqualTo(UserRoles.EndUser));
            Assert.That(resp.Users, Has.Some.Property("Role").EqualTo(UserRoles.Admin));
        }

        [Test]
        public async Task UserGetInRoleEndUser()
        {
            BuildResponse("users.json?role=end-user", "UserGetInRoleEndUser.json", HttpMethod.Get);

            var resp = await Client.Support.Users.GetInRoles(UserRoles.EndUser);

            Assert.That(resp.Users, Has.All.Property("Role").EqualTo(UserRoles.EndUser));
        }
    }
}
