// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Requester of a Ticket
    /// </summary>
    public class Requester
    {
        /// <summary>
        /// Optional
        /// The default locale for the user
        /// </summary>
        public long LocaleId { get; set; }

        /// <summary>
        /// Optional only for existing user.
        /// Users Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Users Email address
        /// </summary>
        public string Email { get; set; }
    }
}
