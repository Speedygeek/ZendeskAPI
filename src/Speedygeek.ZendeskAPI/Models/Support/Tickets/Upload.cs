using System;
using System.Collections.Generic;
using System.Text;

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
