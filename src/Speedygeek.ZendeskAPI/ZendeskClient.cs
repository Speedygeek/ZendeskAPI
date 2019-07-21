// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Speedygeek.ZendeskAPI.Operations.Support;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// Entry Point to the full Zendesk API SDK.
    /// </summary>
    public class ZendeskClient : IZendeskClient
    {
        private readonly IRESTClient _restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZendeskClient"/> class.
        /// </summary>
        /// <param name="restClient"> client used to make HTTP request</param>
        public ZendeskClient(IRESTClient restClient) => _restClient = restClient;

        private Lazy<ISupportOperations> SupportLazy => new Lazy<ISupportOperations>(() => new SupportOperations(_restClient));

        /// <inheritdoc />
        public ISupportOperations Support => SupportLazy.Value;
    }
}
