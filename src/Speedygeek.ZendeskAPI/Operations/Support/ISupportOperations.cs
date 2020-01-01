// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Support Operations
    /// </summary>
    public interface ISupportOperations
    {
        /// <summary>
        /// Ticket Operations
        /// </summary>
        ITicketOperations Tickets { get; }

        /// <summary>
        /// Attachment Operations
        /// </summary>
        IAttachmentOperations Attachments { get; }

        /// <summary>
        /// User Operations
        /// </summary>
        IUserOperations Users { get; }
    }
}
