// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Speedygeek.ZendeskAPI.Configuration
{
    /// <summary>
    /// Container for knowing how to setup authentication
    /// </summary>
    public class OAuthAccessTokenCredentials : ICredentialsProvider
    {
        private const string Message = "Parameter can not be null, empty or whitespace";
        private const string Scheme = "Bearer";
        private readonly string _accessToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthAccessTokenCredentials"/> class.
        /// </summary>
        /// <param name="accessToken">OAuth access token</param>
        /// <exception cref="System.ArgumentException">Thrown when accessToekn is null, empty or whitespace</exception>
        public OAuthAccessTokenCredentials(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException(Message, nameof(accessToken));
            }

            _accessToken = accessToken;
        }

        /// <inheritdoc />
        public Task ConfigureHttpClient(HttpClient client)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, _accessToken);
            return Task.CompletedTask;
        }
    }
}
