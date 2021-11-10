// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Agents following a Ticket
    /// </summary>
    public class Follower
    {
        /// <summary>
        /// User ID of agent
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// User Email of agent
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Update action
        /// </summary>
       // [DefaultValue(FollowerAction.None)]
        public FollowerAction Action { get; set; } = FollowerAction.None;
    }
}
