// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support.Enums
{
    /// <summary>
    /// Job Statuses
    /// </summary>
    public enum JobStatuses
    {
        /// <summary>
        /// Default value (invalid)
        /// </summary>
        None = 0,

        /// <summary>
        /// Queued Status
        /// </summary>
        Queued = 1,

        /// <summary>
        /// Working Status
        /// </summary>
        Working = 2,

        /// <summary>
        /// Failed Status
        /// </summary>
        Failed = 3,

        /// <summary>
        /// Completed Status
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Killed Status
        /// </summary>
        Killed = 5,
    }
}
