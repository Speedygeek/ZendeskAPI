// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Models.Base;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Paged list of Tickets
    /// </summary>
    public class TicketListResponse : ListResponseBase
    {
        /// <summary>
        /// Requested Tickets
        /// </summary>
        public IList<Ticket> Tickets { get; }
    }
}
