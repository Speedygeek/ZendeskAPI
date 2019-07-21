// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// The Thumbnail for an <see cref="Attachment"/>
    /// </summary>
    public class Thumbnail
    {
        /// <summary>
        /// The name of the image file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// A full URL where the attachment image file can be downloaded
        /// </summary>
        public Uri ContentUrl { get; set; }

        /// <summary>
        /// The content type of the image file. Example value: image/png
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The size of image file in bytes
        /// </summary>
        public long Size { get; set; }
    }
}
