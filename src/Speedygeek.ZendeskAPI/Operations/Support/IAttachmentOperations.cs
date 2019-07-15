using System;
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
        /// <param name="timeOut">time out override</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The <see cref="UploadResponse"/></returns>
        Task<UploadResponse> UploadAttachment(ZenFile file, string token = null, TimeSpan timeOut = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Upload Attachments
        /// </summary>
        /// <param name="files">files to be upload</param>
        /// <param name="token">tracking token</param>
        /// <param name="timeOut">timeout override</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The <see cref="UploadResponse"/></returns>
        Task<UploadResponse> UploadAttachments(IEnumerable<ZenFile> files, string token = null, TimeSpan timeOut = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a given <see cref="Attachment"/>
        /// </summary>
        /// <param name="attachment"><see cref="Attachment"/> to download</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>a <see cref="ZenFile"/> of the downloaded file</returns>
        Task<ZenFile> DownloadAttachment(Attachment attachment, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a given upload
        /// </summary>
        /// <param name="token">tracking token</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> a <see cref="bool"/> true if successfully removed</returns>
        Task<bool> DeleteUpload(string token, CancellationToken cancellationToken = default);
    }
}
