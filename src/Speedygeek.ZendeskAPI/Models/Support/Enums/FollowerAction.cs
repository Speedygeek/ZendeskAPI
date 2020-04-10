// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// acctions for updating the Follower list
    /// </summary>
    public enum FollowerAction
    {
        /// <summary>
        /// No change to the Follower
        /// </summary>
        None = 0,

        /// <summary>
        /// Add Follower
        /// </summary>
        Put = 1,

        /// <summary>
        /// Remove Follower
        /// </summary>
        Delete = 2,
    }
}
