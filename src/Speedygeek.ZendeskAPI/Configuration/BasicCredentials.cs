// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Speedygeek.ZendeskAPI.Configuration
{
    /// <summary>
    /// Container for knowing how to setup authentication
    /// </summary>
    public class BasicCredentials : ICredentialsProvider
    {
        private const string Message = "Parameter can not be null, empty or whitespace";
        private readonly string _userName;
        private readonly string _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicCredentials"/> class.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        public BasicCredentials(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException(Message, nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(Message, nameof(password));
            }

            _userName = userName;
            _password = password;
        }

        /// <summary>
        /// Configure the <see cref="HttpClient"/> authentication header
        /// </summary>
        /// <param name="client">to update</param>
        /// <returns><see cref="Task"/> when completed</returns>
        public Task ConfigureHttpClientAsync(HttpClient client)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userName}:{_password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            return Task.CompletedTask;
        }
    }
}
