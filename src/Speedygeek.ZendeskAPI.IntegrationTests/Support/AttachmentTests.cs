using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.IntegrationTests.Base;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.IntegrationTests.Support
{
    public class AttachmentTests : BaseTests
    {
        [Test]
        public async Task AttachmentCreateDownloadDelete()
        {
            var file = new FileInfo(Path.Combine(TestContext.CurrentContext.GetDataDirectoryPath(), "testupload.txt"));

            var token = string.Empty;
            Attachment attachment = null;
            using (var stream = file.Open(FileMode.Open))
            {
                using var zenFile = new ZenFile { ContentType = "text/plain", FileName = "testupload.txt", FileData = stream };
                var resp = await Client.Support.Attachments.Upload(zenFile).ConfigureAwait(false);

                token = resp.Upload.Token;
                Assert.That(resp.Upload.Token, Is.Not.Null);

                attachment = resp.Upload.Attachments[0];
            }

            using (var zenFile = await Client.Support.Attachments.Download(attachment).ConfigureAwait(false))
            {
                Assert.That(zenFile.FileData, Is.Not.Null);
                using var reader = new StreamReader(zenFile.FileData);
                var content = reader.ReadToEnd();
                Assert.That(content, Is.EqualTo("Just a sample file."));
            }

            var result = await Client.Support.Attachments.Delete(token).ConfigureAwait(false);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AttachmentsCreateDelete()
        {
            var file = new FileInfo(Path.Combine(TestContext.CurrentContext.GetDataDirectoryPath(), "testupload.txt"));

            using var stream = file.Open(FileMode.Open);
            var stream1 = new MemoryStream();
            var stream2 = new MemoryStream();
            await stream.CopyToAsync(stream1).ConfigureAwait(false);
            stream.Position = 0;
            await stream.CopyToAsync(stream2).ConfigureAwait(false);

            var files = new List<ZenFile> {
                    new ZenFile { ContentType = "text/plain", FileName = "testupload1.txt", FileData = stream1 },
                    new ZenFile { ContentType = "text/plain", FileName = "testupload2.txt", FileData = stream2 } };

            var resp = await Client.Support.Attachments.Upload(files).ConfigureAwait(false);

            var token = resp.Upload.Token;
            Assert.That(resp.Upload.Token, Is.Not.Null);

            var result = await Client.Support.Attachments.Delete(token).ConfigureAwait(false);
            Assert.That(result, Is.True);

            files.ForEach(z => z.Dispose());
        }
    }
}
