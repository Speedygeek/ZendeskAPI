// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Speedygeek.ZendeskAPI.Configuration;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// Options for setting up <see cref="ZendeskClient"/>
    /// </summary>
    public class ZenOptions
    {
        /// <summary>
        /// Account subdomain
        /// </summary>
        public string SubDomain { get; set; }

        /// <summary>
        /// Provides the configuration for setting up authentication.
        /// </summary>
        public ICredentialsProvider Credentials { get; set; }

        /// <summary>
        /// Set to the Max Timeout wanted. (TimeSpan.Zero ignored)
        /// </summary>
        public TimeSpan TimeOut { get; set; } = TimeSpan.Zero;
    }
}
