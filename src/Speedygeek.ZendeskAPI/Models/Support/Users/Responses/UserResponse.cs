// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// <see cref="User"/>
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Requested <see cref="User"/>
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Total number of open tickets assigned to the user.
        /// </summary>
        public long OpenTicketCount { get; set; }
    }
}
