// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Configuration;
using Speedygeek.ZendeskAPI.Contract;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.Http
{
    /// <summary>
    /// A chainable wrapper around HttpClient and RestClient.
    /// </summary>
    public class ZenRequest : IHttpSettingsContainer
    {
        private SettingsBase _settings;
        private RestClient _client;
        private Uri _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZenRequest"/> class.
        /// </summary>
        /// <param name="url">The URL to call with this <see cref="ZenRequest"/> instance.</param>
        public ZenRequest(string url = null)
        {
            _url = new Uri(url);
        }

        /// <summary>
        /// Gets or sets the ZendeskClient to use when sending the request.
        /// </summary>
        internal RestClient Client
        {
            get => _client ?? ((Url != null) ? new RestClient(Url.ToString()) : null);
            set
            {
                _client = value;
                ResetDefaultSettings();
            }
        }

        /// <summary>
        /// Gets or sets the URL to be called.
        /// </summary>
        public Uri Url
        {
            get => _url;
            set
            {
                _url = value;
                ResetDefaultSettings();
            }
        }

        /// <summary>
        /// Gets collection of headers sent on this request.
        /// </summary>
        public IDictionary<string, object> Headers { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the SettingsBase used by this request.
        /// </summary>
        public SettingsBase Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new SettingsBase();
                    ResetDefaultSettings();
                }

                return _settings;
            }

            set
            {
                _settings = value;
                ResetDefaultSettings();
            }
        }

        private void ResetDefaultSettings()
        {
            if (_settings != null)
            {
                _settings.Defaults = Client?.Settings;
            }
        }

        /// <summary>
        /// Creates and asynchronously sends an HttpRequestMethod.
        /// Mainly used to implement higher-level extension methods (GetJsonAsync, etc).
        /// </summary>
        /// <param name="verb">The HTTP method used to make the request.</param>
        /// <param name="content">Contents of the request body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="completionOption">The HttpCompletionOption used in the request. Optional.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        internal async Task<HttpResponseMessage> SendAsync(HttpMethod verb, HttpContent content = null, CancellationToken cancellationToken = default, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var request = new HttpRequestMessage(verb, Url) { Content = content };
            var call = new HttpCall { ZenRequest = this, HttpRequest = request };
            request.SetHttpCall(call);

            var cancellationTokenWithTimeout = cancellationToken;
            CancellationTokenSource timeoutTokenSource = null;

            if (Settings.Timeout.HasValue)
            {
                timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutTokenSource.CancelAfter(Settings.Timeout.Value);
                cancellationTokenWithTimeout = timeoutTokenSource.Token;
            }

            call.StartedUtc = DateTime.UtcNow;
            try
            {
                Headers.Merge(Client.Headers);
                foreach (var header in Headers)
                {
                    request.SetHeader(header.Key, header.Value);
                }

                call.HttpResponse = await Client.HttpClient.SendAsync(request, completionOption, cancellationTokenWithTimeout).ConfigureAwait(false);
                call.HttpResponse.RequestMessage = request;

                if (call.Succeeded)
                {
                    return call.HttpResponse;
                }

                throw new ZenException(call, null);
            }
            catch (Exception ex)
            {
                return HandleException(call, ex, cancellationToken);
            }
            finally
            {
                request.Dispose();
                timeoutTokenSource?.Dispose();

                call.EndedUtc = DateTime.UtcNow;
            }
        }

        internal static HttpResponseMessage HandleException(HttpCall call, Exception ex, CancellationToken token)
        {
            call.Exception = ex;

            if (ex is OperationCanceledException && !token.IsCancellationRequested)
            {
                throw new ZenTimeoutException(call, ex);
            }

            if (ex is ZenException)
            {
                throw ex;
            }

            throw new ZenException(call, ex);
        }
    }
}
