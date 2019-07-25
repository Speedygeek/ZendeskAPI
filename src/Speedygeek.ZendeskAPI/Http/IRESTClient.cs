// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;
using Speedygeek.ZendeskAPI.Serialization;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// Internal configuration container
    /// </summary>
    public interface IRESTClient
    {
        /// <summary>
        /// Serializer
        /// </summary>
        ISerializer Serializer { get; }

        /// <summary>
        /// options used when setting up client
        /// </summary>
        ZenOptions Options { get; }

        /// <summary>
        /// <see cref="HttpClient" /> used to make request
        /// </summary>
        HttpClient Client { get; }
    }
}
