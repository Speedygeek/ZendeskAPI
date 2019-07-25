// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;
using Microsoft.Extensions.Options;
using Speedygeek.ZendeskAPI.Serialization;

namespace Speedygeek.ZendeskAPI
{
    internal class RESTClient : IRESTClient
    {
        public RESTClient(HttpClient client, IOptions<ZenOptions> options, ISerializer serializer)
        {
            Options = options.Value;
            Client = client;
            Serializer = serializer;
        }

        public ISerializer Serializer { get; }

        public ZenOptions Options { get; }

        public HttpClient Client { get; }
    }
}
