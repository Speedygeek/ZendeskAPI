using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;
using System.Net.Http;
using System.Threading;
using Speedygeek.ZendeskAPI.Utilities;
using System.Linq;
using System.Net;
using System.IO;
using Speedygeek.ZendeskAPI.Models.Support.Tickets.Responses;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <inheritdoc />
    public class AttachmentOperations : BaseOperations, IAttachmentOperations
    {
        private const string BASEREQUESTURI = "uploads.json";

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentOperations"/> class.
        /// </summary>
        /// <param name="restClient">client used to make HTTP calls with</param>
        public AttachmentOperations(IRESTClient restClient) : base(restClient)
        {
        }

        /// <inheritdoc />
        public Task<UploadResponse> UploadAttachment(ZenFile file, string token = default, TimeSpan timeOut = default, CancellationToken cancellationToken = default)
        {
            var queryString = new Dictionary<string, string> { { "filename", file.FileName } };

            if (!string.IsNullOrWhiteSpace(token))
            {
                queryString.Add(nameof(token), token);
            }

            if(file.FileData.Position != 0)
            {
                file.FileData.Position = 0;
            }

            return SendAync<UploadResponse>(HttpMethod.Post, BASEREQUESTURI.BuildQueryString(queryString), file, cancellationToken, timeOut);
        }

        /// <inheritdoc />
        public async Task<UploadResponse> UploadAttachments(IEnumerable<ZenFile> files, string token = default, TimeSpan timeOut = default, CancellationToken cancellationToken = default)
        {
            var first = files.First();
            if (first != null)
            {
                var resp = await UploadAttachment(first, token, timeOut, cancellationToken).ConfigureAwait(false);

                var respToken = resp.Upload.Token;

                var task = new List<Task<UploadResponse>>();
                var otherFiles = files.Skip(1);
                foreach (var file in otherFiles)
                {
                    task.Add(UploadAttachment(file, respToken, timeOut, cancellationToken));
                }

                await Task.WhenAll(task).ConfigureAwait(false);

                return await task.LastOrDefault().ConfigureAwait(false);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<ZenFile> DownloadAttachment(Attachment attachment, CancellationToken cancellationToken = default)
        {
            var file = new ZenFile { FileName = attachment.FileName, ContentType = attachment.ContentType };

            var fileContent = await SendAync<Stream>(HttpMethod.Get, attachment.ContentUrl,
                   async (HttpResponseMessage response) =>
                   {
                       var stream = new MemoryStream();
                       var fileStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                       await fileStream.CopyToAsync(stream).ConfigureAwait(false);
                       stream.Position = 0;
                       return stream;
                   }, cancellationToken).ConfigureAwait(false);

            file.FileData = fileContent;

            return file;
        }

        /// <inheritdoc />
        public Task<bool> DeleteUpload(string token, CancellationToken cancellationToken = default)
        {
            return SendAync(HttpMethod.Delete, $"uploads/{token}.json", (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.NoContent || resp.StatusCode == HttpStatusCode.OK); }, cancellationToken);
        }
    }
}
