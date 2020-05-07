using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Operations;
using Speedygeek.ZendeskAPI.Operations.Support;
using Speedygeek.ZendeskAPI.UnitTests.Base;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.UnitTests.Support
{
    [TestFixture]
    public class UserTests : TestBase
    {

        [Test]
        public async Task UserGetWithAbilities()
        {
            BuildResponse($"users/{Settings.SampleUserId}.json?include=abilities", "UserGetWithAbilities.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetAsync(Settings.SampleUserId, UserSideloads.Abilities);

            Assert.That(resp.User.Id, Is.EqualTo(Settings.SampleUserId));
            Assert.That(resp.User.Abilities.UserId, Is.EqualTo(Settings.SampleUserId));
        }

        [Test]
        public async Task UserGetAllPage2()
        {
            BuildResponse($"users.json?page=2&per_page=50", "UserGetAllPage2.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetAllAsync(new PageParameters { PageNumber = 2, PerPage = 50 });

            Assert.That(resp.Page, Is.EqualTo(2));
            Assert.That(resp.Users, Has.Count.EqualTo(50));
        }

        [Test]
        public async Task UserGetInRoles()
        {
            BuildResponse("users.json?role[]=end-user&role[]=admin", "UserGetInRoles.json", HttpMethod.Get);

            var resp = await Client.Support.Users.GetInRolesAsync(UserRoles.EndUser | UserRoles.Admin);

            Assert.That(resp.Users, Has.Some.Property("Role").EqualTo(UserRoles.EndUser));
            Assert.That(resp.Users, Has.Some.Property("Role").EqualTo(UserRoles.Admin));
        }

        [Test]
        public async Task UserGetInRoleEndUser()
        {
            BuildResponse("users.json?role=end-user", "UserGetInRoleEndUser.json", HttpMethod.Get);

            var resp = await Client.Support.Users.GetInRolesAsync(UserRoles.EndUser);

            Assert.That(resp.Users, Has.All.Property("Role").EqualTo(UserRoles.EndUser));
        }

        [Test]
        public async Task UserGetInCustomRole()
        {
            BuildResponse($"users.json?permission_set={Settings.CustomRoleId}", "UserGetInCustomRole.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetInCustomRoleAsync(Settings.CustomRoleId);

            Assert.That(resp.Users[0].CustomRoleId, Is.EqualTo(Settings.CustomRoleId));
        }

        [Test]
        public async Task UserGetByGroup()
        {
            BuildResponse($"groups/{Settings.GroupId}/users.json", "UserGetByGroup.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetByGroupAsync(Settings.GroupId).ConfigureAwait(false);

            Assert.That(resp.Users, Has.All.Property("DefaultGroupId").EqualTo(Settings.GroupId));
        }

        [Test]
        public async Task UserGetByOrganization()
        {
            BuildResponse($"organizations/{Settings.OrganizationId}/users.json", "UserGetByOrganization.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetByOrganizationAsync(Settings.OrganizationId).ConfigureAwait(false);

            Assert.That(resp.Users, Has.All.Property("OrganizationId").EqualTo(Settings.OrganizationId));
        }

        [Test]
        public async Task UserGetMany()
        {
            var ids = new List<long> { 1038035616, 1038037576, 1038319516 };

            BuildResponse($"users/show_many.json?ids={ids.ToCsv()}", "UserGetMany.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetManyAsync(ids);

            var returnedIds = resp.Users.Select(u => u.Id);

            Assert.That(returnedIds, Is.EqualTo(ids));
        }

        [Test]
        public void UserGetManyWithOver100()
        {
            var random = new Random();
            var ids = new List<long>();
            for (var i = 0; i < 102; i++)
            {
                ids.Add(random.Next());
            }

            Assert.That(async () => { var resp = await Client.Support.Users.GetManyAsync(ids).ConfigureAwait(false); },
                           Throws.ArgumentException.With.Message.EqualTo($"API will not accept a list over 100 items long{Environment.NewLine}Parameter name: ids"));
        }

        [Test]
        public async Task UserGetManyByExternalIds()
        {
            var externalIds = new List<string> { "972cc0aa8d064903a30c619517d1e888", "9d99c444c738422ea565230b0fab0c1f", "9897534532114" };

            BuildResponse($"users/show_many.json?external_ids={externalIds.ToCsv()}", "UserGetManyByExternalIds.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetManyByExternalIdsAsync(externalIds).ConfigureAwait(false);

            var returnedExternalIds = resp.Users.Select(u => u.ExternalId);

            Assert.That(returnedExternalIds, Is.EqualTo(externalIds));
        }

        [Test]
        public void UserGetManyByExternalIdsWithOver100()
        {
            var random = new Random();
            var externalIds = new List<string>();
            for (var i = 0; i < 102; i++)
            {
                externalIds.Add(random.Next().ToInvariantString());
            }

            Assert.That(async () => { var resp = await Client.Support.Users.GetManyByExternalIdsAsync(externalIds).ConfigureAwait(false); },
                           Throws.ArgumentException.With.Message.EqualTo($"API will not accept a list over 100 items long{Environment.NewLine}Parameter name: externalIds"));
        }

        [Test]
        public async Task UserGetRelatedInfo()
        {
            BuildResponse($"users/{Settings.SampleUserId}/related.json", "UserGetRelatedInfo.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetRelatedInfoAsync(Settings.SampleUserId).ConfigureAwait(false);

            Assert.That(resp.UserRelated.RequestedTickets, Is.EqualTo(3111));
        }

        [Test]
        public async Task UserCreate()
        {
            var user = new User()
            {
                Name = "test user10",
                Email = "test10@test.com",
            };

            BuildResponse($"users.json", "UserCreate.json", HttpMethod.Post);
            var resp = await Client.Support.Users.CreateAsync(user);

            Assert.That(resp.User.Email, Is.EqualTo(user.Email));
            Assert.That(resp.User.Id, Is.Not.Zero);
        }

        [Test]
        public async Task UserCreateMany()
        {
            var users = new List<User> {
                new User {  Name = Guid.NewGuid().ToString("N"), Email = $"{Guid.NewGuid():N}@corp.com" },
                new User {  Name = Guid.NewGuid().ToString("N"), Email = $"{Guid.NewGuid():N}@corp.com" },
                new User {  Name = Guid.NewGuid().ToString("N"), Email = $"{Guid.NewGuid():N}@corp.com" }
            };

            BuildResponse($"users/create_many.json", "UserCreateMany.json", HttpMethod.Post);
            var resp = await Client.Support.Users.CreateManyAsync(users).ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public async Task UserCreateOrUpdate()
        {
            var user = new User { Name = "9a452bf9d59b4e5083900acf543d1ace", Email = "1acd6df62aa9466ebbc15f7714cb3e75@corp.com", Verified = true };

            BuildResponse($"users/create_or_update.json", "UserCreateOrUpdate.json", HttpMethod.Post);
            var resp = await Client.Support.Users.CreateOrUpdateAsync(user).ConfigureAwait(false);

            Assert.That(resp.User.Name, Is.EqualTo(user.Name));
            Assert.That(resp.User.Verified, Is.True);
        }

        [Test]
        public async Task UserCreateOrUpdateMany()
        {
            var users = new List<User> {
                new User {  Name = Guid.NewGuid().ToString("N"), Email = $"{Guid.NewGuid():N}@corp.com" },
                new User {  Name = Guid.NewGuid().ToString("N"), Email = $"{Guid.NewGuid():N}@corp.com" },
                new User {  Name = Guid.NewGuid().ToString("N"), Email = $"{Guid.NewGuid():N}@corp.com" }
            };

            BuildResponse($"users/create_or_update_many.json", "UserCreateOrUpdateMany.json", HttpMethod.Post);
            var resp = await Client.Support.Users.CreateOrUpdateManyAsync(users).ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public void UserCreateOrUpdateManyWithOver100()
        {
            var random = new Random();
            var users = new List<User>();
            for (var i = 0; i < 102; i++)
            {
                users.Add(new User { Name = random.Next().ToInvariantString() });
            }

            Assert.That(async () => { var resp = await Client.Support.Users.CreateOrUpdateManyAsync(users).ConfigureAwait(false); },
                           Throws.ArgumentException.With.Message.EqualTo($"API will not accept a list over 100 items long{Environment.NewLine}Parameter name: users"));
        }

        [Test]
        public async Task UserMerge()
        {
            var fromId = 397490051371L;
            var toId = 1038037576L;

            BuildResponse($"users/{fromId}/merge.json", "UserMerge.json", HttpMethod.Put);
            var resp = await Client.Support.Users.MergeAsync(fromId, toId).ConfigureAwait(false);

            Assert.That(resp.User.Id, Is.EqualTo(toId));
        }

        [Test]
        public async Task UserUpdate()
        {
            var id = 1038319516;

            BuildResponse($"users/{id}.json", "UserUpdateGet.json", HttpMethod.Get);
            var respGet = await Client.Support.Users.GetAsync(id);

            var user = respGet.User;
            user.Verified = true;

            BuildResponse($"users/{id}.json", "UserUpdate.json", HttpMethod.Put);
            var resp = await Client.Support.Users.UpdateAsync(user).ConfigureAwait(false);

            Assert.That(resp.User.Verified, Is.True);
        }

        [Test]
        public async Task UserUpdateBatch()
        {
            var ids = new List<long> { 1038319516, 1038037576, 1038319516 };

            BuildResponse($"users/show_many.json?ids={ids.ToCsv()}", "UserUpdateBatchGetMany.json", HttpMethod.Get);
            var respGet = await Client.Support.Users.GetManyAsync(ids);

            var users = respGet.Users.ToList();
            users.ForEach(u => u.Verified = true);

            BuildResponse($"users/update_many.json", "UserUpdateBatch.json", HttpMethod.Put);
            var resp = await Client.Support.Users.UpdateBatchAsync(users).ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public void UserUpdateBatchWithOver100()
        {
            var random = new Random();
            var users = new List<User>();
            for (var i = 0; i < 102; i++)
            {
                users.Add(new User { Name = random.Next().ToInvariantString() });
            }

            Assert.That(async () => { var resp = await Client.Support.Users.UpdateBatchAsync(users).ConfigureAwait(false); },
                           Throws.ArgumentException.With.Message.EqualTo($"API will not accept a list over 100 items long{Environment.NewLine}Parameter name: users"));
        }

        [Test]
        public async Task UserUpdateBulk()
        {
            var ids = new List<long> { 1038319516, 1038037576, 1038319516 };

            BuildResponse($"users/update_many.json?ids={ids.ToCsv()}", "UserUpdateBulk.json", HttpMethod.Put);
            var resp = await Client.Support.Users.UpdateBulkAsync(new User { Verified = false }, ids).ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public void UserUpdateBulkWithOver100()
        {
            var random = new Random();
            var ids = new List<long>();
            for (var i = 0; i < 102; i++)
            {
                ids.Add(random.Next());
            }

            Assert.That(async () => { var resp = await Client.Support.Users.UpdateBulkAsync(new User { Verified = true }, ids).ConfigureAwait(false); },
                           Throws.ArgumentException.With.Message.EqualTo($"API will not accept a list over 100 items long{Environment.NewLine}Parameter name: ids"));
        }

        [Test]
        public void UserUpdateBulkNullUser()
        {
            var ids = new List<long> { 1038319516, 1038037576, 1038319516 };

            Assert.That(async () => { var resp = await Client.Support.Users.UpdateBulkAsync(null, ids).ConfigureAwait(false); },
                           Throws.ArgumentNullException.With.Message.EqualTo($"Value cannot be null.{Environment.NewLine}Parameter name: user"));
        }

        [Test]
        public async Task UserUpdateBulkByExternalIds()
        {
            var externalIds = new List<string> { "972cc0aa8d064903a30c619517d1e888", "9d99c444c738422ea565230b0fab0c1f", "9897534532114" };

            BuildResponse($"users/update_many.json?external_ids={externalIds.ToCsv()}", "UserUpdateBulkByExternalIds.json", HttpMethod.Put);
            var resp = await Client.Support.Users.UpdateBulkAsync(new User { Verified = false }, externalIds).ConfigureAwait(false);
            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public void UserUpdateBulkByExternalIdsNullUser()
        {
             var externalIds = new List<string> { "972cc0aa8d064903a30c619517d1e888", "9d99c444c738422ea565230b0fab0c1f", "9897534532114" };

            Assert.That(async () => { var resp = await Client.Support.Users.UpdateBulkAsync(null, externalIds).ConfigureAwait(false); },
                           Throws.ArgumentNullException.With.Message.EqualTo($"Value cannot be null.{Environment.NewLine}Parameter name: user"));
        }

        [Test]
        public void UserUpdateBulkByExternalIdsWithOver100()
        {
            var random = new Random();
            var externalIds = new List<string>();
            for (var i = 0; i < 102; i++)
            {
                externalIds.Add(random.Next().ToString());
            }

            Assert.That(async () => { var resp = await Client.Support.Users.UpdateBulkAsync(new User { Verified = true }, externalIds).ConfigureAwait(false); },
                           Throws.ArgumentException.With.Message.EqualTo($"API will not accept a list over 100 items long{Environment.NewLine}Parameter name: externalIds"));
        }

        [Test]
        public async Task UserDeleteBulk()
        {
            var ids = new List<long> { 1038319516, 1038037576, 1038319516 };

            BuildResponse($"users/destroy_many.json?ids={ids.ToCsv()}", "UserDeleteBulk.json", HttpMethod.Delete);
            var resp = await Client.Support.Users.DeleteBulkAsync(ids).ConfigureAwait(false);

            Assert.That(resp, Is.True);
        }

        [Test]
        public async Task UserDeleteBulkByExternalIds()
        {
            var externalIds = new List<string> { "972cc0aa8d064903a30c619517d1e888", "9d99c444c738422ea565230b0fab0c1f", "9897534532114" };

            BuildResponse($"users/destroy_many.json?ids={externalIds.ToCsv()}", "UserDeleteBulkByExternalIds.json", HttpMethod.Delete);
            var resp = await Client.Support.Users.DeleteBulkAsync(externalIds).ConfigureAwait(false);

            Assert.That(resp, Is.True);
        }

        [Test]
        public async Task UserDelete()
        {
            long id = 397242072651;

            BuildResponse($"users/{id}.json", "UserDelete.json", HttpMethod.Delete);
            var resp = await Client.Support.Users.DeleteAsync(id);

            Assert.That(resp.User.Active, Is.False);
            Assert.That(resp.User.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task UserSearch()
        {
            var phone = "(111) 222-3333";
            BuildResponse($"users/search.json?query=phone:*{phone}", "UserSearch.json", HttpMethod.Get);
            var resp = await Client.Support.Users.SearchAsync($"phone:*{phone}");

            Assert.That(resp.Users.Count, Is.Not.Zero);
            Assert.That(resp.Users[0].Phone, Is.EqualTo(phone));
        }


        [Test]
        public async Task UserSetPhoto()
        {
            BuildResponse($"users/{Settings.SampleUserId}.json", "UserSetPhoto.json", HttpMethod.Put);

            var path = Path.Combine(TestContext.CurrentContext.GetDataDirectoryPath(), "gracehoppertocat3.jpg");
            using var photofile = new MemoryStream();
            using (var file = File.OpenRead(path))
            {
                await file.CopyToAsync(photofile);
            }

            var photo = new ZenFile { ContentType = "image/jpeg", FileData = photofile, FileName = "gracehoppertocat3.jpg" };

            var resp = await Client.Support.Users.SetPhotoAsync(Settings.SampleUserId, photo);

            Assert.That(resp.User.Photo.ContentUrl, Is.Not.Null);
            Assert.That(resp.User.Photo.Size, Is.Not.Zero);
        }

        [Test]
        public async Task UserGetDeleted()
        {
            BuildResponse($"deleted_users.json", "UserGetDeleted.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetDeletedAsync().ConfigureAwait(false);

            Assert.That(resp.DeletedUsers, Has.Count.EqualTo(100));
        }

        [Test]
        public async Task UserGetDeletedById()
        {
            long id = 389979842772;
            BuildResponse($"deleted_users/{id}.json", "UserGetDeletedById.json", HttpMethod.Get);
            var resp = await Client.Support.Users.GetDeletedAsync(id).ConfigureAwait(false);

            Assert.That(resp.DeletedUser.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task UserPermanentlyDelete()
        {
            long id = 389979842772;
            BuildResponse($"deleted_users/{id}.json", "UserPermanentlyDelete.json", HttpMethod.Delete);
            var resp = await Client.Support.Users.PermanentlyDeleteAsync(id).ConfigureAwait(false);

            Assert.That(resp.DeletedUser.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task UserGetNextPage()
        {
            BuildResponse($"users.json?page=1&per_page=50", "UserGetNextPage1.json", HttpMethod.Get);
            var resp1 = await Client.Support.Users.GetAllAsync(new PageParameters { PageNumber = 1, PerPage = 50 }).ConfigureAwait(false);

            BuildResponse($"users.json?page=2&per_page=50", "UserGetNextPage2.json", HttpMethod.Get);
            var resp2 = await Client.Support.Users.GetNextPageAsync(resp1.NextPage).ConfigureAwait(false);

            Assert.That(resp2.Page, Is.EqualTo(2));
            Assert.That(resp2.Users, Has.Count.EqualTo(50));
        }

        [Test]
        public void UserGetNextPageNull()
        {
            Assert.That(async () => { var resp = await Client.Support.Users.GetNextPageAsync(null).ConfigureAwait(false); },
                        Throws.ArgumentNullException);
        }
    }
}
