// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Text;

namespace Speedygeek.ZendeskAPI.Http
{
    /// <summary>
    /// Provides HTTP content based on a string, with the string itself captured to a property
    /// so it can be read without affecting the read-once content stream.
    /// </summary>
    public class CapturedStringContent : StringContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedStringContent"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        public CapturedStringContent(string content, Encoding encoding = null, string mediaType = null)
            : base(content, encoding, mediaType)
        {
            Content = content;
        }

        /// <summary>
        /// Gets the content body captured as a string. Can be read multiple times (unlike the content stream).
        /// </summary>
        public string Content { get; }
    }
}
