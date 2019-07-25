// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// State of the ticket.
    /// </summary>
    public enum TicketStatus
    {
        /// <summary>
        /// Default value (invalid)
        /// </summary>
        None = 0,

        /// <summary>
        /// New Status
        /// </summary>
        New = 1,

        /// <summary>
        /// Open Status
        /// </summary>
        Open = 2,

        /// <summary>
        /// Pending Status
        /// </summary>
        Pending = 3,

        /// <summary>
        /// Hold Status
        /// </summary>
        Hold = 4,

        /// <summary>
        /// Solved Status
        /// </summary>
        Solved = 5,

        /// <summary>
        /// Closed Status
        /// </summary>
        Closed = 6,
    }
}
