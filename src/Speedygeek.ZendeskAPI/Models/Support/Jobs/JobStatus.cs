// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// A status record is created when somebody kicks off a job such as updating multiple tickets.
    /// </summary>
    public class JobStatus
    {
        /// <summary>
        /// Automatically assigned when the entity is created
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The API URL of this entity
        /// </summary>
        public Uri URL { get; }

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
