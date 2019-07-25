﻿// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Speedygeek.ZendeskAPI.Configuration;
using Speedygeek.ZendeskAPI.Serialization;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// Extensions for setting up <see cref="ZendeskClient"/> in <see cref="IServiceCollection" />
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="ZendeskClient"/> and configure it to use user name and password.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to update</param>
        /// <param name="subDomain"> SubDomain for your Zendesk account. example: https://{subDomain}.zendesk.com</param>
        /// <param name="userName">User name for account</param>
        /// <param name="password">password to authenticate with</param>
        /// <returns>an updated <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddZendeskClient(this IServiceCollection services, string subDomain, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(subDomain))
            {
                throw new ArgumentNullException(nameof(subDomain));
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            return services.AddZendeskClient(options =>
            {
                options.SubDomain = subDomain;
                options.Credentials = new BasicCredentials(userName, password);
            });
        }

        /// <summary>
        /// Add <see cref="ZendeskClient"/> and configure it
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to update</param>
        /// <param name="configureOptions"> action that configures the <see cref="ZenOptions"/></param>
        /// <returns>an updated <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddZendeskClient(this IServiceCollection services, Action<ZenOptions> configureOptions)
        {
            services.Configure<ZenOptions>(configureOptions);

            services.AddScoped<IZendeskClient, ZendeskClient>();
            services.AddScoped<ISerializer, JsonDotNetSerializer>();

            services.AddHttpClient<IRESTClient, RESTClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ZenOptions>>().Value;
                client.BaseAddress = new Uri($"https://{options.SubDomain}.zendesk.com/api/v2/");
                options.Credentials.ConfigureHttpClient(client).ConfigureAwait(true).GetAwaiter().GetResult();

                if (options.TimeOut != TimeSpan.Zero)
                {
                    client.Timeout = options.TimeOut;
                }
            });
            return services;
        }
    }
}
