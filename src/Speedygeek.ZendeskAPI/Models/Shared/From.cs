// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models
{
    /// <summary>
    /// From metadata
    /// </summary>
    public class From
    {
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Will be populated when channel is Voice.
        /// </summary>
        public string FormattedPhone { get; set; }

        /// <summary>
        /// Will be populated when channel is Voice.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Will be populated when Ticket is a follow-up ticket.
        /// </summary>
        public long TicketId { get; set; }

        /// <summary>
        /// Set when channel is email
        /// </summary>
        public string OriginalRecipients { get; set; }

        /// <summary>
        /// Set when channel is follow-up ticket
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
    }
}
