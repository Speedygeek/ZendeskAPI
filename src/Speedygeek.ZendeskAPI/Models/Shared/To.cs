// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models
{
    public class To
    {
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// set when channel is voice
        /// </summary>
        public string FormattedPhone { get; set; }

        /// <summary>
        /// set when channel is voice
        /// </summary>
        public string Phone { get; set; }
    }
}
