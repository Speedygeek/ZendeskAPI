// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Speedygeek.ZendeskAPI.Models.Support.Jobs;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Response  for JobStatus
    /// </summary>
    public class JobStatusResponse
    {
        /// <summary>
        /// Requested JobStatus
        /// </summary>
        public JobStatus JobStatus { get; set; }
    }
}
