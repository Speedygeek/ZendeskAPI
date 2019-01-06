// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Speedygeek.ZendeskAPI.Configuration
{
    /// <summary>
    /// A static container for global configuration settings.
    /// </summary>
    public static class ZendeskConfig
    {
        private static readonly object _configLock = new object();

        private static readonly Lazy<GlobalSettings> _settings = new Lazy<GlobalSettings>(() => new GlobalSettings());

        /// <summary>
        /// Gets globally configured ZendeskAPI settings. Should normally be written to by calling ZendeskConfig.Configure once application at startup.
        /// </summary>
        public static GlobalSettings GlobalSettings => _settings.Value;

        /// <summary>
        /// Provides thread-safe access to ZendeskConfig's global configuration settings. Should only be called once at application startup.
        /// </summary>
        /// <param name="configAction">the action to perform against the GlobalSettings.</param>
        public static void Configure(Action<GlobalSettings> configAction)
        {
            lock (_configLock)
            {
                configAction(GlobalSettings);
            }
        }
    }
}
