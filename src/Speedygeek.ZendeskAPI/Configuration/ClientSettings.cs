// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Speedygeek.ZendeskAPI.Contract;

namespace Speedygeek.ZendeskAPI.Configuration
{
    /// <summary>
    /// Client-level settings.
    /// </summary>
    public class ClientSettings : SettingsBase
    {
        /// <summary>
        /// Gets or sets the time to keep the underlying HTTP/TCP connection open. When expired, a Connection: close header
        /// is sent with the next request, which should force a new connection and DSN lookup to occur on the next call.
        /// Default is null, effectively disabling the behavior.
        /// </summary>
        public TimeSpan? ConnectionLeaseTimeout
        {
            get => Get(() => ConnectionLeaseTimeout);
            set => Set(() => ConnectionLeaseTimeout, value);
        }

        /// <summary>
        /// Gets or sets a factory used to create the HttpClient and HttpMessageHandler used for HTTP calls.
        /// </summary>
        public IHttpClientFactory HttpClientFactory
        {
            get => Get(() => HttpClientFactory);
            set => Set(() => HttpClientFactory, value);
        }
    }
}
