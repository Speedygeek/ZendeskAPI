// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Speedygeek.ZendeskAPI.Operations.Support;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// Describes the basic setup of a <see cref="ZendeskClient"/>
    /// </summary>
    public interface IZendeskClient
    {
        /// <summary>
        /// Ticket Operations
        /// </summary>
        ITicketOperations Tickets { get; }
    }
}
