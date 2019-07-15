// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    public class Attachment : ZenEntity
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// A full URL where the attachment file can be downloaded
        /// </summary>
        public string ContentUrl { get; set; }

        /// <summary>
        /// The content type of the file. Example value: image/png
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The size of file in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// An array of Photo objects.
        /// Note that thumbnails do not have thumbnails.
        /// </summary>
        public IList<Thumbnail> Thumbnails { get; set; }

        /// <summary>
        /// If true, the attachment is excluded from the attachment list
        /// and the attachment's URL can be referenced within the comment of a ticket.
        /// Default is false
        /// </summary>
        public bool Inline { get; set; }

    }
}
