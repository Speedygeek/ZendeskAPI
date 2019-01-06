// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using Speedygeek.ZendeskAPI.Configuration;
using Speedygeek.ZendeskAPI.Contract;
using Speedygeek.ZendeskAPI.Testing;

namespace Speedygeek.ZendeskAPI
{
    internal class RestClient : IHttpSettingsContainer
    {
        private readonly object _connectionLeaseLock = new object();
        private ClientSettings _settings;
        private Lazy<HttpClient> _httpClient;
        private Lazy<HttpMessageHandler> _httpMessageHandler;

        private DateTime? _clientCreatedAt;
        private HttpClient _zombieClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL associated with this client.</param>
        public RestClient(string baseUrl = null)
        {
            _httpClient = new Lazy<HttpClient>(CreateHttpClient);
            _httpMessageHandler = new Lazy<HttpMessageHandler>(() => Settings.HttpClientFactory.CreateMessageHandler());
            BaseUrl = baseUrl;
        }

        public ClientSettings Settings
        {
            get => _settings ?? (_settings = new ClientSettings());
            set => _settings = value;
        }

        /// <summary>
        /// Gets or sets base URL associated with this client.
        /// </summary>
        public string BaseUrl { get; set; }

        public IDictionary<string, object> Headers { get; } = new Dictionary<string, object>();

        public HttpClient HttpClient => HttpTest.Current?.HttpClient ?? GetHttpClient();

        SettingsBase IHttpSettingsContainer.Settings
        {
            get => Settings;
            set => Settings = value as ClientSettings;
        }

        /// <summary>
        /// Gets a value indicating whether this instance (and its underlying HttpClient) has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the HttpMessageHandler to be used in subsequent HTTP calls.
        /// </summary>
        public HttpMessageHandler HttpMessageHandler => HttpTest.Current?.HttpMessageHandler ?? _httpMessageHandler?.Value;

        private HttpClient GetHttpClient()
        {
            if (ConnectionLeaseExpired())
            {
                lock (_connectionLeaseLock)
                {
                    if (ConnectionLeaseExpired())
                    {
                        // when the connection lease expires, force a new HttpClient to be created, but don't
                        // dispose the old one just yet - it might have pending requests. Instead, it becomes
                        // a zombie and is disposed on the _next_ lease timeout, which should be safe.
                        _zombieClient?.Dispose();
                        _zombieClient = _httpClient.Value;
                        _httpClient = new Lazy<HttpClient>(CreateHttpClient);
                        _httpMessageHandler = new Lazy<HttpMessageHandler>(() => Settings.HttpClientFactory.CreateMessageHandler());
                        _clientCreatedAt = DateTime.UtcNow;
                    }
                }
            }

            return _httpClient.Value;
        }

        private HttpClient CreateHttpClient()
        {
            var cli = Settings.HttpClientFactory.CreateHttpClient(HttpMessageHandler);
            _clientCreatedAt = DateTime.UtcNow;
            return cli;
        }

        private bool ConnectionLeaseExpired()
        {
            // for thread safety, capture these to temp variables
            var createdAt = _clientCreatedAt;
            var timeout = Settings.ConnectionLeaseTimeout;

            return
                _httpClient.IsValueCreated &&
                createdAt.HasValue &&
                timeout.HasValue &&
                DateTime.UtcNow - createdAt.Value > timeout.Value;
        }

        /// <summary>
        /// Disposes the underlying HttpClient and HttpMessageHandler.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            if (_httpMessageHandler?.IsValueCreated == true)
            {
                _httpMessageHandler.Value.Dispose();
            }

            if (_httpClient?.IsValueCreated == true)
            {
                _httpClient.Value.Dispose();
            }

            IsDisposed = true;
        }
    }
}
