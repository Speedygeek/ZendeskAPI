// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;

namespace Speedygeek.ZendeskAPI.Http
{
    internal static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Set a header on this HttpRequestMessage (default), or its Content property if it's a known content-level header.
        /// No validation. Overwrites any existing value(s) for the header.
        /// </summary>
        /// <param name="request">The HttpRequestMessage.</param>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public static void SetHeader(this HttpRequestMessage request, string name, object value)
        {
            request.Headers.TryAddWithoutValidation(name, new[] { value.ToString() });
        }

        /// <summary>
        /// Associate an HttpCall object with this request
        /// </summary>
        internal static void SetHttpCall(this HttpRequestMessage request, HttpCall call)
        {
            if (request?.Properties != null)
            {
                request.Properties["ZenHttpCall"] = call;
            }
        }

        /// <summary>
        /// Get the HttpCall associated with this request, if any.
        /// </summary>
        internal static HttpCall GetHttpCall(this HttpRequestMessage request)
        {
            if (request?.Properties != null && request.Properties.TryGetValue("ZenHttpCall", out var obj) && obj is HttpCall call)
            {
                return call;
            }

            return null;
        }
    }
}
