// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// User Related Information
    /// </summary>
    public class UserRelated
    {
        /// <summary>
        /// Count of assigned tickets
        /// </summary>
        public long AssignedTickets { get; set; }

        /// <summary>
        /// Count of requested tickets
        /// </summary>
        public long RequestedTickets { get; set; }

        /// <summary>
        /// Count of collaborated tickets
        /// </summary>
        public long CcdTickets { get; set; }

        /// <summary>
        /// Count of organization subscriptions
        /// </summary>
        public long OrganizationSubscriptions { get; set; }

        /// <summary>
        /// Count of topics (Web portal only)
        /// </summary>
        public long Topics { get; set; }

        /// <summary>
        /// Count of comments on topics (Web portal only)
        /// </summary>
        public long TopicComments { get; set; }

        /// <summary>
        /// Count of votes (Web portal only)
        /// </summary>
        public long Votes { get; set; }

        /// <summary>
        /// Count of subscriptions (Web portal only)
        /// </summary>
        public long Subscriptions { get; set; }

        /// <summary>
        /// Count of entry subscriptions (Web portal only)
        /// </summary>
        public long EntrySubscriptions { get; set; }

        /// <summary>
        /// Count of forum subscriptions (Web portal only)
        /// </summary>
        public long ForumSubscriptions { get; set; }
    }
}
