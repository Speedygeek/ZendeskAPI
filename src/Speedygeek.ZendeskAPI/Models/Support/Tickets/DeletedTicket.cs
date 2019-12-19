// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Model for working with soft deleted Tickets
    /// </summary>
    public class DeletedTicket
    {
        /// <summary>
        /// Automatically assigned when the entity is created
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Ticket Subject at time of delete
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// User that deleted ticket
        /// </summary>
        public Actor Actor { get; set; }

        /// <summary>
        /// DateTime ticket was deleted
        /// </summary>
        public DateTimeOffset DeletedAt { get; set; }

        /// <summary>
        /// Ticket status when deleted
        /// </summary>
        public TicketStatus PreviousState { get; set; }
    }
}
