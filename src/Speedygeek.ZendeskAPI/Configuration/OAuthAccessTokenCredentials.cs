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
        private readonly string _endUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthAccessTokenCredentials"/> class.
        /// </summary>
        /// <param name="accessToken">OAuth access token</param>
        /// <param name="endUserId">end users Id number or email address</param>
        /// <exception cref="System.ArgumentException">Thrown when accessToekn or endUserId is null, empty or whitespace</exception>
        /// <see href="https://develop.zendesk.com/hc/en-us/articles/360001068647-Making-API-requests-on-behalf-of-end-users-Zendesk-Support"/>
        public OAuthAccessTokenCredentials(string accessToken, string endUserId)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException(Message, nameof(accessToken));
            }

            if (string.IsNullOrWhiteSpace(endUserId))
            {
                throw new ArgumentException(Message, nameof(endUserId));
            }

            _accessToken = accessToken;
            _endUserId = endUserId;
        }

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
        public Task ConfigureHttpClientAsync(HttpClient client)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, _accessToken);
            if (!string.IsNullOrWhiteSpace(_endUserId))
            {
                client.DefaultRequestHeaders.Add("X-On-Behalf-Of", _endUserId);
            }

            return Task.CompletedTask;
        }
    }
}
