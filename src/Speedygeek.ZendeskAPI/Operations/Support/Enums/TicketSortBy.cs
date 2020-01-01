// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Ticket Sort By <see href="https://developer.zendesk.com/rest_api/docs/support/tickets#available-parameters"/>
    /// </summary>
    public enum TicketSortBy
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Assignee
        /// </summary>
        Assignee = 1,

        /// <summary>
        /// Assignee Name
        /// </summary>
        [EnumMember(Value = "assignee.name")]
        AssigneeName = 2,

        /// <summary>
        /// Created At Date
        /// </summary>
        CreatedAt = 3,

        /// <summary>
        /// Group
        /// </summary>
        Group = 4,

        /// <summary>
        /// Id
        /// </summary>
        Id = 5,

        /// <summary>
        /// Locale
        /// </summary>
        Locale = 6,

        /// <summary>
        /// Requester
        /// </summary>
        Requester = 7,

        /// <summary>
        /// Requester Name
        /// </summary>
        [EnumMember(Value = "requester.name")]
        RequesterName = 8,

        /// <summary>
        /// Status
        /// </summary>
        Status = 9,

        /// <summary>
        /// Subject
        /// </summary>
        Subject = 10,

        /// <summary>
        /// Updated At
        /// </summary>
        UpdateAt = 11,
    }
}
