// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Comment Types
    /// </summary>
    public enum CommentType
    {
        /// <summary>
        /// Comment none
        /// </summary>
        None = 0,

        /// <summary>
        /// Comment type
        /// </summary>
        Comment = 1,

        /// <summary>
        /// Voice Comment
        /// </summary>
        VoiceComment = 3,
    }
}
