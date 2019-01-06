// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Configuration;

namespace Speedygeek.ZendeskAPI.Contract
{
    /// <summary>
    /// Defines state for request and clients
    /// </summary>
    internal interface IHttpSettingsContainer
    {
        /// <summary>
        /// Gets or sets the SettingsBase object used by this client.
        /// </summary>
        SettingsBase Settings { get; set; }

        /// <summary>
        /// Gets collection of headers sent on all requests using this client.
        /// </summary>
        IDictionary<string, object> Headers { get; }
    }
}
