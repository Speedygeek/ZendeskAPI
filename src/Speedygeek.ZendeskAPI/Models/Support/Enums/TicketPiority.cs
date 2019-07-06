// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Possible Ticket Priorities
    /// </summary>
    public enum TicketPriority
    {
        /// <summary>
        /// Default value (invalid)
        /// </summary>
        None = 0,

        /// <summary>
        ///  Low Priority
        /// </summary>
        Low = 1,

        /// <summary>
        /// Normal Priority
        /// </summary>
        Normal = 2,

        /// <summary>
        /// High Priority
        /// </summary>
        High = 3,

        /// <summary>
        /// Urgent Priority
        /// </summary>
        Urgent = 4,
    }
}
