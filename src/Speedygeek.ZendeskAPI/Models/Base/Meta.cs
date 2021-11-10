// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace Speedygeek.ZendeskAPI.Models.Base
{
    /// <summary>
    /// Cursor Pagination meta data
    /// <a href="https://developer.zendesk.com/documentation/developer-tools/pagination/paginating-through-lists-using-cursor-pagination/">See Cursor Pagination</a>
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// Pagination are more Items to load.
        /// </summary>
        [JsonInclude]
        public bool HasMore { get; private set; }

        /// <summary>
        /// Request parameter of the subsequent request to retrieve the next page of results.
        /// </summary>
        [JsonInclude]
        public long AfterCursor { get; private set; }

        /// <summary>
        /// Request parameter of the subsequent request to retrieve the previous page of results.
        /// </summary>
        [JsonInclude]
        public long BeforeCursor { get; private set; }
    }
}
