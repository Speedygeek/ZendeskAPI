// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
