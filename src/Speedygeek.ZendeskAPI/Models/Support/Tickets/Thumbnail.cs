// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    public class Thumbnail
    {
        /// <summary>
        /// The name of the image file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// A full URL where the attachment image file can be downloaded
        /// </summary>
        public string ContentUrl { get; set; }

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
