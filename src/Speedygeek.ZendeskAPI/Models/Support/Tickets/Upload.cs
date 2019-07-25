// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Upload
    /// </summary>
    public class Upload
    {
        /// <summary>
        /// Upload tracking token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Attachments uploaded
        /// </summary>
        public IList<Attachment> Attachments { get; set; }
    }
}
