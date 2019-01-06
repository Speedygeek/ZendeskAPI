// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;

namespace Speedygeek.ZendeskAPI.Contract
{
    /// <summary>
    /// Interface defining creation of HttpClient and HttpMessageHandler used in all HTTP calls.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Defines how HttpClient should be instantiated and configured by default.
        /// </summary>
        /// <param name="httpMessageHandler">The HttpMessageHandler used to construct the HttpClient.</param>
        /// <returns>  an <see cref="HttpClient"/> </returns>
        HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler);

        /// <summary>
        /// Defines how the Message Handler should be created.
        /// </summary>
        /// <returns> an <see cref="HttpMessageHandler"/></returns>
        HttpMessageHandler CreateMessageHandler();
    }
}
