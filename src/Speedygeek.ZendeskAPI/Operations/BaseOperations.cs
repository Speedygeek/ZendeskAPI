// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// High level methods for making HTTP request.
    /// </summary>
    public abstract class BaseOperations
    {
        private const string JSON_TYPE = "application/json";
        private const HttpStatusCode TooManyRequests = (HttpStatusCode)429;
        private readonly IRESTClient _restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseOperations"/> class.
        /// </summary>
        /// <param name="restClient"> REST Client used for all HTTP calls</param>
        public BaseOperations(IRESTClient restClient)
        {
            _restClient = restClient;
        }

        /// <summary>
        /// main send method
        /// </summary>
        /// <typeparam name="TResponse">response type</typeparam>
        /// <param name="httpMethod">HTTP method for the HTTP call</param>
        /// <param name="requestSuffix">relative URL for the resource </param>
        /// <param name="body">data to be sent to zendesk</param>
        /// <param name="cancellationToken">cancellation to support async</param>
        /// <returns> <typeparamref name="TResponse"/></returns>
        protected async Task<TResponse> SendAync<TResponse>(HttpMethod httpMethod, string requestSuffix, object body = null, CancellationToken cancellationToken = default)
        {
            using var httpRequestMessage = new HttpRequestMessage(httpMethod, requestSuffix);
            if (body is ZenFile zenFile)
            {
                cancellationToken.ThrowIfCancellationRequested();
                httpRequestMessage.Content = new StreamContent(zenFile.FileData);
                httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(zenFile.ContentType);
            }
            else if (body != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                httpRequestMessage.Content = new StringContent(_restClient.Serializer.Serialize(body), Encoding.UTF8, JSON_TYPE);
            }

            cancellationToken.ThrowIfCancellationRequested();
            using var response = await _restClient.Client.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            TResponse result = default;
            if (response.IsSuccessStatusCode)
            {
                cancellationToken.ThrowIfCancellationRequested();
                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(true);
                stream.Seek(0, SeekOrigin.Begin);
                result = _restClient.Serializer.Deserialize<TResponse>(stream);
            }
            else if (response.StatusCode == TooManyRequests)
            {
                var retryAfter = response.Headers.GetValues("Retry-After").FirstOrDefault();

                throw new HttpRequestException($"HTTP status 429 To Many Requests; you may retry after {retryAfter}");
            }
            else if (!response.IsSuccessStatusCode)
            {
                var bodyString = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                var message = $"Error {response.StatusCode} details: HEADERS: {response.Headers} BODY: {bodyString}";

                throw new HttpRequestException(message);
            }

            return result;
        }

        /// <summary>
        /// Send Method with Callback
        /// </summary>
        /// <typeparam name="TResult">response type</typeparam>
        /// <param name="httpMethod">HTTP method for the HTTP call</param>
        /// <param name="requestSuffix">relative URL for the resource </param>
        /// <param name="callBack"> work to be done with the request</param>
        /// <param name="cancellationToken">cancellation to support async</param>
        /// <returns> <typeparamref name="TResult"/></returns>
        protected async Task<TResult> SendAync<TResult>(HttpMethod httpMethod, string requestSuffix, Func<HttpResponseMessage, Task<TResult>> callBack, CancellationToken cancellationToken = default)
        {
            using var httpRequestMessage = new HttpRequestMessage(httpMethod, requestSuffix);
            cancellationToken.ThrowIfCancellationRequested();
            using var response = await _restClient.Client.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            TResult result = default;
            if (response.IsSuccessStatusCode)
            {
                result = await (callBack?.Invoke(response)).ConfigureAwait(true);
            }
            else if (response.StatusCode == TooManyRequests)
            {
                var retryAfter = response.Headers.GetValues("Retry-After").FirstOrDefault();

                throw new HttpRequestException($"HTTP status 429 To Many Requests; you may retry after {retryAfter}");
            }
            else if (!response.IsSuccessStatusCode)
            {
                var bodyString = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                var message = $"Error {response.StatusCode} details: HEADERS: {response.Headers} BODY: {bodyString}";

                throw new HttpRequestException(message);
            }

            return result;
        }
    }
}
