// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Models.Base;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// List of Deleted users
    /// </summary>
    public class DeletedUserListResponse : ListResponseBase
    {
        /// <summary>
        /// Lists deleted users, including permanently deleted users.
        /// </summary>
        public IList<User> DeletedUsers { get; set; }
    }
}
