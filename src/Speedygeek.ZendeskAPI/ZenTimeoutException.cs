// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Speedygeek.ZendeskAPI.Http;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// An exception that is thrown when an HTTP call made times out.
    /// </summary>
    public class ZenTimeoutException : ZenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZenTimeoutException"/> class.
        /// </summary>
        /// <param name="call">The HttpCall instance.</param>
        /// <param name="inner">The inner exception.</param>
        public ZenTimeoutException(HttpCall call, Exception inner)
            : base(call, BuildMessage(call), inner)
        {
        }

        private static string BuildMessage(HttpCall call)
        {
            return $"Call timed out: {call}";
        }
    }
}
