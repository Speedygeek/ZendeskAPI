// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Text.Json.Serialization;

namespace Speedygeek.ZendeskAPI.Models.Base
{
    /// <summary>
    /// Cursor Pagination Links
    /// </summary>
    public class Links
    {
        /// <summary>
        /// Next page Uri
        /// </summary>
        [JsonInclude]
        public Uri Next { get; private set; }

        /// <summary>
        /// Previous page Uri
        /// </summary>
        [JsonInclude]
        public Uri Prev { get; private set; }
    }
}
