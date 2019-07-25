// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models
{
    /// <summary>
    /// Information about how a entity was created.
    /// </summary>
    public class Via
    {
        /// <summary>
        /// This tells you how the ticket or event was created.
        /// Examples: "web", "mobile", "rule", "system"
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// For some channels a source object gives more information
        /// about how or why the ticket or event was created
        /// </summary>
        public Source Source { get; set; }
    }
}
