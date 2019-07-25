// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models
{
    /// <summary>
    /// Source of an Entity
    /// </summary>
    public class Source
    {
        /// <summary>
        /// Where the entity Came from
        /// </summary>
        public From From { get; set; }

        /// <summary>
        /// Data about the entity
        /// </summary>
        public To To { get; set; }

        /// <summary>
        /// Relative source
        /// </summary>
        public string Rel { get; set; }
    }
}
