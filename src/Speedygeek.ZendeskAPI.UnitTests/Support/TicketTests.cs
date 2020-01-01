using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Models.Support.Enums;
using Speedygeek.ZendeskAPI.Operations.Support;
using Speedygeek.ZendeskAPI.UnitTests.Base;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.UnitTests.Support
{
    [TestFixture]
    public class TicketTests : TestBase
    {
        [Test]
        public async Task TicketCreateReadUpdateDelete()
        {
            BuildResponse("tickets.json", "createTicket.json", HttpMethod.Post);

            var newTicket = new Ticket()
            {
                Subject = "my printer is on fire",
                Comment = new Comment { Body = "HELP" },
                Priority = TicketPriority.Urgent,
            };

            var createResp = await Client.Support.Tickets.Create(newTicket).ConfigureAwait(false);

            Assert.That(createResp, Is.Not.Null);
            Assert.That(createResp.Ticket.Id, Is.EqualTo(24116));

            BuildResponse("tickets/24116.json", "createTicket.json");

            var id = createResp.Ticket.Id;
            var getResp = await Client.Support.Tickets.Get(id).ConfigureAwait(false);

            Assert.That(getResp, Is.Not.Null);
            Assert.That(getResp.Ticket.Id, Is.EqualTo(id));

            BuildResponse("tickets/24116.json", "update_24116_Ticket.json", HttpMethod.Post);
            var ticket = getResp.Ticket;
            ticket.Subject = "this is an updated ticket";
            ticket.Comment = new Comment { Body = "Updated subject", Public = false };
            var updateResp = await Client.Support.Tickets.Update(ticket).ConfigureAwait(false);

            Assert.That(updateResp.Ticket.Subject, Is.EqualTo(ticket.Subject));

            BuildResponse("tickets/24116.json", string.Empty, HttpMethod.Delete);

            var deleteResp = await Client.Support.Tickets.Delete(id).ConfigureAwait(false);

            Assert.That(deleteResp, Is.True);
        }

        [Test]
        public void TicketCreateNull()
        {
            Assert.That(async () => { var resp = await Client.Support.Tickets.Create(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public async Task TicketCreateMany()
        {
            BuildResponse("tickets/create_many.json", "ticketsCreate_many.json", HttpMethod.Post);
            var tickets = new List<Ticket> { new Ticket { Comment = new Comment { Body = "Test 1" } }, new Ticket { Comment = new Comment { Body = "Test 2" } },
                    new Ticket { Comment = new Comment { Body = "Test 3" } } };

            var resp = await Client.Support.Tickets.CreateMany(tickets).ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public async Task TicketsByOrg()
        {
            BuildResponse("organizations/22560572/tickets.json?page=1&per_page=50", "organization_22560572_tickets.json");

            var resp = await Client.Support.Tickets.GetByOrganization(22560572, new TicketPageParams { PerPage = 50 }, TicketSideloads.None).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(50));
            Assert.That(resp.NextPage, Is.Not.Null);
            Assert.That(resp.PerPage, Is.EqualTo(50));
            Assert.That(resp.TotalPages, Is.EqualTo(27));
        }

        [Test]
        public async Task TicketsGetAll()
        {
            BuildResponse("tickets.json", "tickets.json");

            var resp = await Client.Support.Tickets.GetAll().ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }

        [Test]
        public async Task TicketsGetAllWithSideLoad()
        {
            BuildResponse("tickets.json?include=users,comment_count", "ticketsWithSideload.json");

            var resp = await Client.Support.Tickets.GetAll(sideload: TicketSideloads.Users | TicketSideloads.CommentCount).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
            Assert.That(resp.Tickets.Any(t => t.CommentCount > 0));
        }

        [Test]
        public async Task TicketsDeleteBulk()
        {
            var ids = new List<long> { 896, 902, 903 };
            BuildResponse($"tickets/destroy_many.json?ids={ids.ToCsv()}", "TicketDeleteBulk.json", HttpMethod.Delete);

            var resp = await Client.Support.Tickets.DeleteBulk(ids).ConfigureAwait(false);

            Assert.That(resp.JobStatus.URL.ToString(), Is.EqualTo($"https://csharpapi.zendesk.com/api/v2/job_statuses/{resp.JobStatus.Id}.json"));
        }

        [Test]
        public void TicketsDeleteBulkWithOver100()
        {
            var random = new Random();
            var ids = new List<long>();
            for (var i = 0; i < 102; i++)
            {
                ids.Add(random.Next());
            }

            Assert.That(async () => { var resp = await Client.Support.Tickets.DeleteBulk(ids).ConfigureAwait(false); },
                Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: ids"));
        }

        [Test]
        public void TicketUpdateNull()
        {
            Assert.That(async () => { var resp = await Client.Support.Tickets.Update(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public void TicketUpdateBatchNull()
        {
            Assert.That(async () => { var resp = await Client.Support.Tickets.UpdateBatch(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public async Task TicketUpdateBatch()
        {
            BuildResponse("tickets.json", "tickets.json");
            var respAll = await Client.Support.Tickets.GetAll().ConfigureAwait(false);
            respAll.Tickets.ToList().ForEach(t => t.Comment = new Comment { Body = $"this is to test batch update: {t.Id}" });

            BuildResponse($"tickets/update_many.json", "TicketsUpdateBatch.json", HttpMethod.Put);
            var resp = await Client.Support.Tickets.UpdateBatch(respAll.Tickets).ConfigureAwait(false);

            Assert.That(resp.JobStatus.URL.ToString(), Is.EqualTo($"https://csharpapi.zendesk.com/api/v2/job_statuses/{resp.JobStatus.Id}.json"));
            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
            //Note: should be updated to check for completion of Job.
        }

        [Test]
        public void TicketUpdateBatchOver100()
        {
            var tickets = new List<Ticket>();
            for (var i = 0; i < 102; i++)
            {
                tickets.Add(new Ticket { Comment = new Comment { Body = $"{i}" } });
            }

            Assert.That(async () => { var resp = await Client.Support.Tickets.UpdateBatch(tickets).ConfigureAwait(false); },
                Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: tickets"));
        }

        [Test]
        public void TicketUpdateBulkOver100()
        {
            var random = new Random();
            var ids = new List<long>();
            for (var i = 0; i < 102; i++)
            {
                ids.Add(random.Next());
            }

            Assert.That(async () => { var resp = await Client.Support.Tickets.UpdateBulk(new Ticket(), ids).ConfigureAwait(false); },
                Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: ids"));
        }

        [Test]
        public void TicketUpdateBulkNull()
        {
            Assert.That(async () => { var resp = await Client.Support.Tickets.UpdateBulk(null, new List<long>()).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public async Task TicketUpdateBulk()
        {
            var ids = new List<long> { 896, 902, 903, 910 };

            BuildResponse($"tickets/update_many.json?ids={ids.ToCsv()}", "TicketsUpdateBulk.json", HttpMethod.Put);
            var resp = await Client.Support.Tickets.UpdateBulk(new Ticket { Comment = new Comment { Body = $"this is to test bulk update" } }, ids).ConfigureAwait(false);

            Assert.That(resp.JobStatus.URL.ToString(), Is.EqualTo($"https://csharpapi.zendesk.com/api/v2/job_statuses/{resp.JobStatus.Id}.json"));
            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public async Task TicketsGetDeleted()
        {
            BuildResponse($"deleted_tickets.json", "TicketDeletedTickets.json", HttpMethod.Get);

            var resp = await Client.Support.Tickets.GetDeleted().ConfigureAwait(false);

            Assert.That(resp.DeletedTickets.Count, Is.EqualTo(100));
            Assert.That(resp.Page, Is.EqualTo(1));
        }

        [Test]
        public async Task TicketsDeletePermanentlyBulk()
        {
            var ids = new List<long> { 896,902,903,25513,25514,25533,25547,25548,25567,25582,25583,25602,25615,25616,25635,25646,25647,
                25666,25681,25682,25701,25716,25717,25736,25751,25752,25771,25786,25787,25806,25821,25822,25841,25889,25890,25909,25506,25507,
                25508,25509,25510,25511,25512,25515,25516,25517,25518,25519,25520,25521,25522,25523,25524,25525,25526,25527,25528,25529,25530,25531,25532,25535,25536,
                25537,25538,25539,25540,25541,25542,25543,25544,25545,25546,25550,25551,25552,25553,25554,25555,25556,25557,25558,25559,25560,25561,25562,25563,25564,25565,
                25566,25569,25570,25571,25572,25573,25574,25575,25576,25926,25927 };
            BuildResponse($"deleted_tickets/destroy_many.json?ids={ids.ToCsv()}", "TicketDeletePermanentlyBulk.json", HttpMethod.Delete);

            var resp = await Client.Support.Tickets.DeletePermanentlyBulk(ids).ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public void TicketsDeletePermanentlyBulkOver100()
        {
            var ids = new List<long> { 896,902,903,25513,25514,25533,25547,25548,25567,25582,25583,25602,25615,25616,25635,25646,25647,
                25666,25681,25682,25701,25716,25717,25736,25751,25752,25771,25786,25787,25806,25821,25822,25841,25889,25890,25909,25506,25507,
                25508,25509,25510,25511,25512,25515,25516,25517,25518,25519,25520,25521,25522,25523,25524,25525,25526,25527,25528,25529,25530,25531,25532,25535,25536,
                25537,25538,25539,25540,25541,25542,25543,25544,25545,25546,25550,25551,25552,25553,25554,25555,25556,25557,25558,25559,25560,25561,25562,25563,25564,25565,
                25566,25569,25570,25571,25572,25573,25574,25575,25576,25926,25927, 1111 };

            Assert.That(async () => { var resp = await Client.Support.Tickets.DeletePermanentlyBulk(ids).ConfigureAwait(false); },
                Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: ids"));
        }

        [Test]
        public async Task TicketDeletePermanently()
        {
            var id = 910L;

            BuildResponse($"tickets/{id}.json", string.Empty, HttpMethod.Delete);

            var deleteResp = await Client.Support.Tickets.Delete(id).ConfigureAwait(false);

            Assert.That(deleteResp, Is.True);

            BuildResponse($"deleted_tickets/{id}.json", "TicketDeletePermanently.json", HttpMethod.Delete);
            var deletePermanentlyResp = await Client.Support.Tickets.DeletePermanently(id).ConfigureAwait(false);

            Assert.That(deletePermanentlyResp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public async Task TicketRestore()
        {
            var id = 930L;

            BuildResponse($"tickets/{id}.json", string.Empty, HttpMethod.Delete);

            var deleteResp = await Client.Support.Tickets.Delete(id).ConfigureAwait(false);

            Assert.That(deleteResp, Is.True);

            BuildResponse($"deleted_tickets/{id}/restore.json", string.Empty, HttpMethod.Put);
            var resp = await Client.Support.Tickets.Restore(id).ConfigureAwait(false);

            Assert.That(resp, Is.True);
        }

        [Test]
        public async Task TicketRestoreBulk()
        {
            BuildResponse($"deleted_tickets.json", "TicketDeletedTickets.json", HttpMethod.Get);

            var deleteResp = await Client.Support.Tickets.GetDeleted().ConfigureAwait(false);
            var ids = deleteResp.DeletedTickets.Select(d => d.Id).ToList();

            BuildResponse($"deleted_tickets/restore_many.json?ids={ids.ToCsv()}", string.Empty, HttpMethod.Put);
            var resp = await Client.Support.Tickets.RestoreBulk(ids).ConfigureAwait(false);

            Assert.That(resp, Is.True);
        }

        [Test]
        public void TicketRestoreBulkOver100()
        {
            var ids = new List<long> { 896,902,903,25513,25514,25533,25547,25548,25567,25582,25583,25602,25615,25616,25635,25646,25647,
                25666,25681,25682,25701,25716,25717,25736,25751,25752,25771,25786,25787,25806,25821,25822,25841,25889,25890,25909,25506,25507,
                25508,25509,25510,25511,25512,25515,25516,25517,25518,25519,25520,25521,25522,25523,25524,25525,25526,25527,25528,25529,25530,25531,25532,25535,25536,
                25537,25538,25539,25540,25541,25542,25543,25544,25545,25546,25550,25551,25552,25553,25554,25555,25556,25557,25558,25559,25560,25561,25562,25563,25564,25565,
                25566,25569,25570,25571,25572,25573,25574,25575,25576,25926,25927, 1111 };

            Assert.That(async () => { var resp = await Client.Support.Tickets.RestoreBulk(ids).ConfigureAwait(false); },
                Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: ids"));
        }

        [Test]
        public async Task TicketGetMany()
        {
            BuildResponse("tickets.json", "ticketsGetAllV2.json");
            var respAll = await Client.Support.Tickets.GetAll().ConfigureAwait(false);
            var ids = respAll.Tickets.Select(t => t.Id).ToList();

            BuildResponse($"tickets/show_many.json?ids={ids.ToCsv()}", "TicketsGetMany.json", HttpMethod.Get);
            var resp = await Client.Support.Tickets.GetMany(ids).ConfigureAwait(false);

            Assert.That(resp.Tickets.Select(t => t.Id).ToList(), Is.EqualTo(ids));
        }

        [Test]
        public void TicketGetManyOver100()
        {
            var ids = new List<long> { 896,902,903,25513,25514,25533,25547,25548,25567,25582,25583,25602,25615,25616,25635,25646,25647,
                25666,25681,25682,25701,25716,25717,25736,25751,25752,25771,25786,25787,25806,25821,25822,25841,25889,25890,25909,25506,25507,
                25508,25509,25510,25511,25512,25515,25516,25517,25518,25519,25520,25521,25522,25523,25524,25525,25526,25527,25528,25529,25530,25531,25532,25535,25536,
                25537,25538,25539,25540,25541,25542,25543,25544,25545,25546,25550,25551,25552,25553,25554,25555,25556,25557,25558,25559,25560,25561,25562,25563,25564,25565,
                25566,25569,25570,25571,25572,25573,25574,25575,25576,25926,25927, 1111 };

            Assert.That(async () => { var resp = await Client.Support.Tickets.GetMany(ids).ConfigureAwait(false); },
                Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: ids"));
        }

        [Test]
        public async Task TicketNextPage()
        {
            BuildResponse("tickets.json", "ticketsGetAllV2.json");
            var respAll = await Client.Support.Tickets.GetAll().ConfigureAwait(false);

            BuildResponse($"tickets.json?page=2", "TicketsNextPage.json", HttpMethod.Get);
            var resp = await Client.Support.Tickets.GetNextPage(respAll.NextPage).ConfigureAwait(false);

            Assert.That(resp.Page, Is.EqualTo(2));
        }

        [Test]
        public void TicketNextPageNull()
        {
            Assert.That(async () => { var resp = await Client.Support.Tickets.GetNextPage(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public async Task TicketRecent()
        {
            BuildResponse($"tickets/recent.json", "TicketsRecent.json");
            var resp = await Client.Support.Tickets.GetRecent().ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task TicketGetByRequestedUser()
        {
            BuildResponse($"users/{Settings.SampleUserId}/tickets/requested.json", "TicketsByRequestedUser.json");
            var resp = await Client.Support.Tickets.GetByRequestedUser(Settings.SampleUserId).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }

        [Test]
        public async Task TicketGetByCarbonCopiedUser()
        {
            BuildResponse($"users/{Settings.SampleUserId}/tickets/ccd.json", "TicketsByCarbonCopiedUser.json");
            var resp = await Client.Support.Tickets.GetByCarbonCopiedUser(Settings.SampleUserId).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TicketGetByAssignedUser()
        {
            BuildResponse($"users/{Settings.SampleUserId}/tickets/assigned.json", "TicketsByAssignedUser.json");
            var resp = await Client.Support.Tickets.GetByAssignedUser(Settings.SampleUserId).ConfigureAwait(false);

            Assert.That(resp.Tickets.Count, Is.EqualTo(100));
        }

        [Test]
        public async Task TicketGetByExternalId()
        {
            BuildResponse($"tickets.json?external_id={Settings.ExternalId}", "TicketsByExternalId.json");
            var resp = await Client.Support.Tickets.GetByExternalId(Settings.ExternalId).ConfigureAwait(false);

            Assert.That(resp.Tickets[0].ExternalId, Is.EqualTo(Settings.ExternalId));
        }

        [Test]
        public async Task TicketMarkAsSpam()
        {
            BuildResponse("tickets.json", "createTicket.json", HttpMethod.Post);

            var newTicket = new Ticket()
            {
                Subject = "my printer is on fire",
                Comment = new Comment { Body = "HELP" },
                Priority = TicketPriority.Urgent,
                Requester = new Requester { Name = "Support", Email = "support@speedy-geek.com" }
            };

            var createResp = await Client.Support.Tickets.Create(newTicket).ConfigureAwait(false);

            BuildResponse($"tickets/{createResp.Ticket.Id}/mark_as_spam.json", string.Empty);
            var resp = await Client.Support.Tickets.MarkAsSpam(createResp.Ticket.Id).ConfigureAwait(false);

            Assert.That(resp, Is.True);
        }

        [Test]
        public async Task TicketMarkAsSpamBulk()
        {
            var ids = new List<long> { 930, 937, 990 };

            BuildResponse($"tickets/mark_many_as_spam.json?ids={ids.ToCsv()}", "TicketMarkAsSpamBulk.json", HttpMethod.Put);
            var resp = await Client.Support.Tickets.MarkAsSpam(ids).ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public void TicketMarkAsSpamBulkOver100()
        {
            var ids = new List<long> { 896,902,903,25513,25514,25533,25547,25548,25567,25582,25583,25602,25615,25616,25635,25646,25647,
                25666,25681,25682,25701,25716,25717,25736,25751,25752,25771,25786,25787,25806,25821,25822,25841,25889,25890,25909,25506,25507,
                25508,25509,25510,25511,25512,25515,25516,25517,25518,25519,25520,25521,25522,25523,25524,25525,25526,25527,25528,25529,25530,25531,25532,25535,25536,
                25537,25538,25539,25540,25541,25542,25543,25544,25545,25546,25550,25551,25552,25553,25554,25555,25556,25557,25558,25559,25560,25561,25562,25563,25564,25565,
                25566,25569,25570,25571,25572,25573,25574,25575,25576,25926,25927, 1111 };

            Assert.That(async () => { var resp = await Client.Support.Tickets.MarkAsSpam(ids).ConfigureAwait(false); },
                    Throws.ArgumentException.With.Message.EqualTo("API will not accept a list over 100 items long\r\nParameter name: ids"));
        }

        [Test]
        public async Task TicketMerge()
        {
            BuildResponse("tickets.json", "TicketsGetAllV2.json");
            var respAll = await Client.Support.Tickets.GetAll().ConfigureAwait(false);
            var targetId = respAll.Tickets.First().Id;

            BuildResponse($"tickets/{targetId}/merge.json", "TicketsMerge.json", HttpMethod.Post);
            var resp = await Client.Support.Tickets.Merge(targetId, new List<long> { respAll.Tickets[1].Id}) .ConfigureAwait(false);

            Assert.That(resp.JobStatus.Status, Is.EqualTo(JobStatuses.Queued));
        }

        [Test]
        public async Task TicketGetIncidents()
        {
            BuildResponse("tickets.json", "TicketCreateProblemTicket.json", HttpMethod.Post);
            var newProblem = new Ticket()
            {
                Subject = "my printer is on fire",
                Comment = new Comment { Body = "Problem help me" },
                Priority = TicketPriority.Urgent,
                Type = TicketType.Problem
            };

            var createProblemResp = await Client.Support.Tickets.Create(newProblem).ConfigureAwait(false);
            
            var problemId = createProblemResp.Ticket.Id;
            var newIncident = new Ticket()
            {
                Subject = "my printer is on fire",
                Comment = new Comment { Body = "Incident help me" },
                Priority = TicketPriority.Urgent,
                Type = TicketType.Incident,
                ProblemId = problemId
            };
            BuildResponse("tickets.json", "TicketCreateIncidentTicket.json", HttpMethod.Post);
            var createIncidentResp = await Client.Support.Tickets.Create(newIncident).ConfigureAwait(false);


            BuildResponse($"tickets/{problemId}/incidents.json", "TicketGetIncidents.json");
            var resp = await Client.Support.Tickets.GetIncidents(problemId).ConfigureAwait(false);

            Assert.That(resp.Tickets[0].Id, Is.EqualTo(createIncidentResp.Ticket.Id));
        }
    }

}
