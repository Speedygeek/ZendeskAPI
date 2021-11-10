// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// response Ticket
    /// </summary>
    public class TicketResponse
    {
        /// <summary>
        /// Requested Ticket
        /// </summary>
        public Ticket Ticket { get; }

        ///// <summary>
        ///// Users related to requested tickets
        ///// </summary>
        // public IList<User> Users { get; }
    }
}
