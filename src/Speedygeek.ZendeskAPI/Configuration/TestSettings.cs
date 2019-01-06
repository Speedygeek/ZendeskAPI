// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Configuration
{
    /// <summary>
    ///  Settings overrides within the context of an HTTP testing.
    /// </summary>
    public class TestSettings : ClientSettings
    {
        /// <summary>
        /// Resets all test settings to their ZendeskAPI default values.
        /// </summary>
        public override void ResetDefaults()
        {
            base.ResetDefaults();

            // HttpClientFactory = new TestHttpClientFactory();
        }
    }
}
