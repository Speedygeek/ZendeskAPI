// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// End user Provided feedback.
    /// </summary>
    public class SatisfactionRating
    {
        /// <summary>
        /// Id for the Satisfction Rating
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Feedback Score
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// End user provided Comment.
        /// </summary>
        public string Comment { get; set; }
    }
}
