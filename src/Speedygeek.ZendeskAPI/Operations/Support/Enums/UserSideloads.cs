// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Options that can be side-loaded with <see cref="User"/>
    /// </summary>
    [Flags]
    public enum UserSideloads
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Abilities
        /// </summary>
        Abilities = 2,

        /// <summary>
        /// Groups
        /// </summary>
        Groups = 4,

        /// <summary>
        /// Organizations
        /// </summary>
        Organizations = 8,

        /// <summary>
        /// Identities
        /// </summary>
        Identities = 16,

        /// <summary>
        /// Roles
        /// </summary>
        Roles = 32,

        /// <summary>
        /// Open Ticket Count
        /// </summary>
        OpenTicketCount = 64,
    }
}
