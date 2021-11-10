// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Models.Base;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Paged list of <see cref="User"/>
    /// </summary>
    public class UserListResponse : ListResponseBase
    {
        /// <summary>
        /// Requested <see cref="User"/>
        /// </summary>
        public IList<User> Users { get; }

        /// <summary>
        /// Total number of open tickets assigned to the user.
        /// </summary>
        public long OpenTicketCount { get; set; }
    }
}
