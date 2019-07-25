// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Proxy object
    /// </summary>
    public class Collaborator
    {
        /// <summary>
        /// User id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string Name { get; set; }
    }
}
