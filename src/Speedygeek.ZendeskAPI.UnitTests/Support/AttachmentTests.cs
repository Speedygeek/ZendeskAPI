using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.UnitTests.Base;

namespace Speedygeek.ZendeskAPI.UnitTests.Support
{
    public class AttachmentTests : TestBase
    {
        [Test]
        public async Task AttachmentCreateDownloadDelete()
        {
           // SaveResponse();
            var file = new FileInfo(Path.Combine(TestContext.CurrentContext.GetDataDirectoryPath(), "testupload.txt"));

            var token = string.Empty;
            Attachment attachment = null;

            BuildResponse("uploads.json?filename=testupload.txt", "uploadTestFile.json", HttpMethod.Post, HttpStatusCode.Created);
            using (var stream = file.Open(FileMode.Open))
            {
                using var zenFile = new ZenFile { ContentType = "text/plain", FileName = "testupload.txt", FileData = stream };
                var resp = await Client.Support.Attachments.Upload(zenFile).ConfigureAwait(false);

                token = resp.Upload.Token;
                Assert.That(resp.Upload.Token, Is.Not.Null);
                attachment = resp.Upload.Attachments[0];
            }

            BuildResponse(attachment.ContentUrl.PathAndQuery, "testupload.txt", HttpMethod.Get, HttpStatusCode.OK);

            using (var zenFile = await Client.Support.Attachments.Download(attachment).ConfigureAwait(false))
            {
                Assert.That(zenFile.FileData, Is.Not.Null);
                using var reader = new StreamReader(zenFile.FileData);
                var content = reader.ReadToEnd();
                Assert.That(content, Is.EqualTo("Just a sample file."));
            }

            BuildResponse($"uploads/{token}.json", string.Empty, HttpMethod.Delete, HttpStatusCode.NoContent);

            var result = await Client.Support.Attachments.Delete(token).ConfigureAwait(false);
            Assert.That(result, Is.True);
        }

        [Test]
        public void AttachmentDownloadNull()
        {
            Assert.That(async () => { var resp = await Client.Support.Attachments.Download(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }
    }
}
