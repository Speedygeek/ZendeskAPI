// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;

namespace Speedygeek.ZendeskAPI.Configuration
{
    /// <summary>
    /// Used to configure credentials
    /// </summary>
    public interface ICredentialsProvider
    {
        /// <summary>
        /// Override to configure the <see cref="HttpClient">HttpClient</see>
        /// </summary>
        /// <param name="client"> HttpClient to configure</param>
        /// <returns>as <see cref="Task"/> when complete</returns>
        Task ConfigureHttpClient(HttpClient client);
    }
}
