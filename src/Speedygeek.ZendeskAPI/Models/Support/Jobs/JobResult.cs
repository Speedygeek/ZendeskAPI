// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// The "results" array in a response lists
    /// the resources that were successfully and
    /// unsuccessfully updated or created after processing.
    /// The results differ depending on the type of job.
    /// </summary>
    public class JobResult
    {
        /// <summary>
        /// The id of the resource the job attempted work with
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The action the job attempted
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Whether the action was successful or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Status Message
        /// </summary>
        public string Status { get; set; }
    }
}
