// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Models.Support.Tickets.Responses;
using Speedygeek.ZendeskAPI.Utilities;

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
        public AttachmentOperations(IRESTClient restClient)
            : base(restClient)
        {
        }

        /// <inheritdoc />
        public Task<UploadResponse> Upload(ZenFile file, string token = default, CancellationToken cancellationToken = default)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var queryString = new Dictionary<string, string> { { "filename", file.FileName } };

            if (!string.IsNullOrWhiteSpace(token))
            {
                queryString.Add(nameof(token), token);
            }

            if (file.FileData.Position != 0)
            {
                file.FileData.Position = 0;
            }

            return SendAync<UploadResponse>(HttpMethod.Post, BASEREQUESTURI.BuildQueryString(queryString), file, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<UploadResponse> Upload(IEnumerable<ZenFile> files, string token = default, CancellationToken cancellationToken = default)
        {
            var first = files.First();
            if (first != null)
            {
                var resp = await Upload(first, token, cancellationToken).ConfigureAwait(false);

                var respToken = resp.Upload.Token;

                var task = new List<Task<UploadResponse>>();
                var otherFiles = files.Skip(1);
                foreach (var file in otherFiles)
                {
                    task.Add(Upload(file, respToken, cancellationToken));
                }

                await Task.WhenAll(task).ConfigureAwait(false);

                return await task.LastOrDefault().ConfigureAwait(false);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<ZenFile> Download(Attachment attachment, CancellationToken cancellationToken = default)
        {
            if (attachment is null)
            {
                throw new ArgumentNullException(nameof(attachment));
            }

            var file = new ZenFile { FileName = attachment.FileName, ContentType = attachment.ContentType };

            var fileContent = await SendAync<Stream>(
                HttpMethod.Get,
                attachment.ContentUrl.ToString(),
                async (HttpResponseMessage response) =>
                   {
                       var stream = new MemoryStream();
                       var fileStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                       await fileStream.CopyToAsync(stream).ConfigureAwait(false);
                       stream.Position = 0;
                       return stream;
                   },
                cancellationToken).ConfigureAwait(false);

            file.FileData = fileContent;

            return file;
        }

        /// <inheritdoc />
        public Task<bool> Delete(string token, CancellationToken cancellationToken = default)
        {
            return SendAync(HttpMethod.Delete, $"uploads/{token}.json", (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.NoContent || resp.StatusCode == HttpStatusCode.OK); }, cancellationToken);
        }
    }
}
