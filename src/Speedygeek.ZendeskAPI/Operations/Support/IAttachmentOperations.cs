// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Models.Support.Tickets.Responses;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Operations for Support/Ticket Attachments
    /// </summary>
    public interface IAttachmentOperations
    {
        /// <summary>
        /// Upload an Attachment to Zendesk.
        /// </summary>
        /// <param name="file">file to upload</param>
        /// <param name="token">tracking token</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The <see cref="UploadResponse"/></returns>
        Task<UploadResponse> Upload(ZenFile file, string token = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Upload Attachments
        /// </summary>
        /// <param name="files">files to be upload</param>
        /// <param name="token">tracking token</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The <see cref="UploadResponse"/></returns>
        Task<UploadResponse> Upload(IEnumerable<ZenFile> files, string token = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a given <see cref="Attachment"/>
        /// </summary>
        /// <param name="attachment"><see cref="Attachment"/> to download</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>a <see cref="ZenFile"/> of the downloaded file</returns>
        Task<ZenFile> Download(Attachment attachment, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a given upload
        /// </summary>
        /// <param name="token">tracking token</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> a <see cref="bool"/> true if successfully removed</returns>
        Task<bool> Delete(string token, CancellationToken cancellationToken = default);
    }
}
