// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "assignee.name")]
        AssigneeName = 2,

        /// <summary>
        /// Created At Date
        /// </summary>
        [Display(Name = "created_at")]
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
        [Display(Name = "requester.name")]
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
        [Display(Name = "updated_at")]
        UpdateAt = 11,
    }
}
