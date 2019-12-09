// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
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
        public JobResult JobStatus { get; set; }
    }
}
