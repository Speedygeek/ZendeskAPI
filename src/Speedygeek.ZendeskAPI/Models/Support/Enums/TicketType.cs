// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Possible Ticket Types
    /// </summary>
    public enum TicketType
    {
        /// <summary>
        /// Default value (invalid)
        /// </summary>
        None = 0,

        /// <summary>
        /// Problem Ticket
        /// </summary>
        Problem = 1,

        /// <summary>
        /// Incident Ticket
        /// </summary>
        Incident = 2,

        /// <summary>
        /// Question Ticket
        /// </summary>
        Question,

        /// <summary>
        /// Task Ticket
        /// </summary>
        Task,
    }
}
