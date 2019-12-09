// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Models.Support.Enums;

namespace Speedygeek.ZendeskAPI.Models.Support.Jobs
{
    /// <summary>
    /// A status record is created when somebody kicks off a job such as updating multiple tickets.
    /// </summary>
    public class JobStatus : ZenEntity
    {
        /// <summary>
        /// The total number of tasks this job is batching through
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Number of tasks that have already been completed
        /// </summary>
        public long Progress { get; set; }

        /// <summary>
        /// The current status.
        /// </summary>
        public JobStatuses Status { get; set; }

        /// <summary>
        /// Message from the job worker, if any
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Result data from processed tasks.
        /// </summary>
        public IList<JobResult> Results { get; set; }
    }
}
