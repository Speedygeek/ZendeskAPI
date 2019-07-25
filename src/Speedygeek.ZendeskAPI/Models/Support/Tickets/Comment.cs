// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using Speedygeek.ZendeskAPI.Operations.Support;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Ticket Comment
    /// </summary>
    public class Comment : ZenEntity
    {
        /// <summary>
        /// The type of this ticket.
        /// </summary>
        public CommentType Type { get; set; }

        /// <summary>
        /// The comment string
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The comment formatted as HTML
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// The comment as plain text
        /// </summary>
        public string PlainBody { get; set; }

        /// <summary>
        /// true if a public comment; false if an internal note.
        /// The initial value set on ticket creation persists for any additional comment unless you change it
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// The id of the comment author. See  <see href="https://developer.zendesk.com/rest_api/docs/support/ticket_comments#author-id"/>
        /// </summary>
        public long AuthorId { get; set; }

        /// <summary>
        /// Update/Create operations only
        /// NOTE: For the upload attachment to succeed when updating a ticket, a comment must be included.
        /// NOTE 2: Use <see cref="AttachmentOperations.Upload(ZenFile, string, CancellationToken)"/>
        /// to get the token first.
        /// </summary>
        public IList<string> Uploads { get; set; }

        /// <summary>
        /// Read operations only
        /// Attachments for this comment
        /// </summary>
        public IList<Attachment> Attachments { get; set; }

        /// <summary>
        /// How the comment was created. See <see href="https://developer.zendesk.com/rest_api/docs/support/ticket_audits#the-via-object"/>
        /// </summary>
        public Via Via { get; set; }

        /// <summary>
        /// System information (web client, IP address, etc.) and comment flags, if any. See <see href="https://developer.zendesk.com/rest_api/docs/support/ticket_comments#comment-flags"/>
        /// </summary>
        public dynamic Metadata { get; set; }
    }
}
