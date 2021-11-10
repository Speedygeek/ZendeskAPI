// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Roles that a user can have
    /// </summary>
    [Flags]
    public enum UserRoles
    {
        /// <summary>
        /// Default value (invalid)
        /// </summary>
        None = 0,

        /// <summary>
        /// End User
        /// </summary>
        EndUser = 1,

        /// <summary>
        /// Agent
        /// </summary>
        Agent = 2,

        /// <summary>
        /// Administrator
        /// </summary>
        Admin = 4,
    }
}
