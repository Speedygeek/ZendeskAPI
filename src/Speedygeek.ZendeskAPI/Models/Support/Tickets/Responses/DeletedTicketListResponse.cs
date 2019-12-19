// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Models.Base;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Response for Deleted Tickets
    /// </summary>
    public class DeletedTicketListResponse : ListResponseBase
    {
        /// <summary>
        /// Requested Deleted Tickets
        /// </summary>
        public IList<DeletedTicket> DeletedTickets { get; set; }
    }
}
