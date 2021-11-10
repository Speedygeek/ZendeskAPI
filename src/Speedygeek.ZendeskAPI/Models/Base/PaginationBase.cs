// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace Speedygeek.ZendeskAPI.Models.Base
{
    /// <summary>
    /// Base class for Cursor Pagination
    /// </summary>
    public class PaginationBase
    {
        /// <summary>
        /// Pagination Meta data
        /// </summary>
        [JsonInclude]
        public Meta Meta { get; private set; }

        /// <summary>
        /// Pagination Links
        /// </summary>
        [JsonInclude]
        public Links Links { get; private set; }
    }
}
