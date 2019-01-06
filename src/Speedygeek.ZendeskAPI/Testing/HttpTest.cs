// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Speedygeek.ZendeskAPI.Configuration;
using Speedygeek.ZendeskAPI.Http;

namespace Speedygeek.ZendeskAPI.Testing
{
    /// <summary>
    /// puts ZendeslAPI into test mode where actual HTTP calls are faked/run in memory.
    /// </summary>
    public class HttpTest : IDisposable
    {
        private readonly Lazy<HttpClient> _httpClient;
        private readonly Lazy<HttpMessageHandler> _httpMessageHandler;
        private bool _disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpTest"/> class.
        /// </summary>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public HttpTest()
        {
            Settings = new TestSettings();

            ResponseQueue = new Queue<HttpResponseMessage>();
            CallLog = new List<HttpCall>();
            _httpClient = new Lazy<HttpClient>(() => Settings.HttpClientFactory.CreateHttpClient(HttpMessageHandler));
            _httpMessageHandler = new Lazy<HttpMessageHandler>(() => Settings.HttpClientFactory.CreateMessageHandler());
            SetCurrentTest(this);
        }

        internal HttpClient HttpClient => _httpClient.Value;

        internal HttpMessageHandler HttpMessageHandler => _httpMessageHandler.Value;

        /// <summary>
        /// Gets or sets the TestSettings object used by this test.
        /// </summary>
        public TestSettings Settings { get; set; }

        /// <summary>
        /// Gets the current HttpTest from the logical (async) call context.
        /// </summary>
        public static HttpTest Current => GetCurrentTest();

        /// <summary>
        /// Gets or sets queue of HttpResponseMessages to be returned in place of real responses during testing.
        /// </summary>
        public Queue<HttpResponseMessage> ResponseQueue { get; set; }

        /// <summary>
        /// Gets list of all (fake) HTTP calls made since this HttpTest was created.
        /// </summary>
        public List<HttpCall> CallLog { get; }

        /// <summary>
        /// Adds an HttpResponseMessage to the response queue.
        /// </summary>
        /// <param name="body">The simulated response body string.</param>
        /// <param name="status">The simulated HTTP status. Default is 200.</param>
        /// <returns>The current HttpTest object (so more responses can be chained).</returns>
        public HttpTest RespondWith(string body, int status = 200)
        {
            return RespondWith(new StringContent(body), status);
        }

        /// <summary>
        /// Adds an HttpResponseMessage to the response queue with the given data serialized to JSON as the content body.
        /// </summary>
        /// <param name="body">The object to be JSON-serialized and used as the simulated response body.</param>
        /// <param name="status">The simulated HTTP status. Default is 200.</param>
        /// <returns>The current HttpTest object (so more responses can be chained).</returns>
        public HttpTest RespondWithJson(object body, int status = 200)
        {
            var content = new CapturedJsonContent(Settings.JsonSerializer.Serialize(body));
            return RespondWith(content, status);
        }

        /// <summary>
        /// Adds an HttpResponseMessage to the response queue.
        /// </summary>
        /// <param name="content">The simulated response body content (optional).</param>
        /// <param name="status">The simulated HTTP status. Default is 200.</param>
        /// <returns>The current HttpTest object (so more responses can be chained).</returns>
        public HttpTest RespondWith(HttpContent content = null, int status = 200)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = (HttpStatusCode)status,
                Content = content,
            };

            ResponseQueue.Enqueue(response);
            return this;
        }

        /// <summary>
        /// Adds a simulated timeout response to the response queue.
        /// </summary>
        /// <returns><see cref="HttpTest"/></returns>
        public HttpTest SimulateTimeout()
        {
            ResponseQueue.Enqueue(new TimeoutResponseMessage());
            return this;
        }

        internal HttpResponseMessage GetNextResponse()
        {
            return ResponseQueue.Any() ? ResponseQueue.Dequeue() : new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty),
            };
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) below.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        ///  Performs application-defined tasks associated with freeing, releasing, or resetting
        ///  unmanaged resources.
        /// </summary>
        /// <param name="disposing">should managed state be cleaned.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    SetCurrentTest(null);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _disposedValue = true;
            }
        }

#pragma warning disable SA1201
        private static readonly System.Threading.AsyncLocal<HttpTest> _test = new System.Threading.AsyncLocal<HttpTest>();

        private static void SetCurrentTest(HttpTest test) => _test.Value = test;

        private static HttpTest GetCurrentTest() => _test.Value;
#pragma warning restore SA1201
    }
}
