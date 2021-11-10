// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Models.Base;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Paged list of Tickets
    /// </summary>
    public class TicketListResponse : PaginationBase
    {
        /// <summary>
        /// Requested Tickets
        /// </summary>
        public List<Ticket> Tickets { get; }

        /// <summary>
        /// Users related to requested tickets
        /// </summary>
        public List<User> Users { get; }
    }
}
