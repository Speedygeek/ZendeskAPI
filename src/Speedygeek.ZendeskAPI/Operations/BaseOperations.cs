// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// High level methods for making HTTP request.
    /// </summary>
    public abstract class BaseOperations
    {
        private const string JSONTYPE = "application/json";
        private const HttpStatusCode TooManyRequests = (HttpStatusCode)429;
        private readonly IRESTClient _restClient;

        /// <summary>
        /// Status 200 OK
        /// </summary>
        protected static readonly Func<HttpResponseMessage, Task<bool>> IsStatus200OK = (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.OK); };

        /// <summary>
        /// Status 204 NoContent Or 200 OK
        /// </summary>
        protected static readonly Func<HttpResponseMessage, Task<bool>> IsStatus204NoContentOr200OK = (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.NoContent || resp.StatusCode == HttpStatusCode.OK); };

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
        protected async Task<TResponse> SendAsync<TResponse>(HttpMethod httpMethod, string requestSuffix, object body = null, CancellationToken cancellationToken = default)
        {
            using var httpRequestMessage = new HttpRequestMessage(httpMethod, requestSuffix);
            cancellationToken.ThrowIfCancellationRequested();
            if (body is ZenFile zenFile)
            {
                if (zenFile.FileData.Position != 0)
                {
                    zenFile.FileData.Position = 0;
                }

                httpRequestMessage.Content = new StreamContent(zenFile.FileData);
                httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(zenFile.ContentType);
            }
            else if (body is Dictionary<string, object> formData)
            {
                httpRequestMessage.Content = BuildFormContent(formData);
            }
            else if (body != null)
            {
                var json = _restClient.Serializer.Serialize(body);
                httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, JSONTYPE);
            }

            cancellationToken.ThrowIfCancellationRequested();
            using var response = await _restClient.Client.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            TResponse result = default;
            if (response.IsSuccessStatusCode)
            {
                cancellationToken.ThrowIfCancellationRequested();
                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(true);
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

        private static HttpContent BuildFormContent(Dictionary<string, object> formData)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var fromContent = new MultipartFormDataContent(Constants.FormBoundary);

            foreach (var item in formData)
            {
                if (item.Value is ZenFile zenFile)
                {
                    if (zenFile.FileData.Position != 0)
                    {
                        zenFile.FileData.Position = 0;
                    }

                    var streamContent = new StreamContent(zenFile.FileData);
                    streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(zenFile.ContentType);
                    fromContent.Add(streamContent, item.Key, zenFile.FileName);
                }
                else
                {
                    var content = new StringContent(item.Value.ToString());

                    fromContent.Add(content, item.Key);
                }
            }
#pragma warning restore CA2000 // Dispose objects before losing scope
            return fromContent;
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
        protected async Task<TResult> SendAyncAsync<TResult>(HttpMethod httpMethod, string requestSuffix, Func<HttpResponseMessage, Task<TResult>> callBack, CancellationToken cancellationToken = default)
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
