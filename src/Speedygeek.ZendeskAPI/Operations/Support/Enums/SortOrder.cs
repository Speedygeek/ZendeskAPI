﻿// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Sort Order for paginated list
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Ascending
        /// </summary>
        [EnumMember(Value = "asc")]
        Ascending = 2,

        /// <summary>
        /// Descending
        /// </summary>
        [EnumMember(Value = "desc")]
        Descending = 4,
    }
}
