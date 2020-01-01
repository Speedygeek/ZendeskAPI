// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Specifies which tickets the user has access to.
    /// </summary>
    public enum UserTicketRestriction
    {
        /// <summary>
        /// Default value (invalid)
        /// </summary>
        None = 0,

        /// <summary>
        /// Null
        /// </summary>
        Null = 1,

        /// <summary>
        /// Organization
        /// </summary>
        Organization = 2,

        /// <summary>
        /// Groups
        /// </summary>
        Groups = 3,

        /// <summary>
        /// Assigned
        /// </summary>
        Assigned = 4,

        /// <summary>
        /// Requested
        /// </summary>
        Requested = 5,
    }
}
