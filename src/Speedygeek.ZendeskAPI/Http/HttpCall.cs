// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Net;
using System.Net.Http;

namespace Speedygeek.ZendeskAPI.Http
{
    /// <summary>
    /// Encapsulates request, response, and other details associated with an HTTP call. Useful for diagnostics
    /// </summary>
    public class HttpCall
    {
        /// <summary>
        /// Gets or sets the ZenRequest associated with this call.
        /// </summary>
        public ZenRequest ZenRequest { get; set; }

        /// <summary>
        /// Gets or sets the HttpRequestMessage associated with this call.
        /// </summary>
        public HttpRequestMessage HttpRequest { get; set; }

        /// <summary>
        /// Gets or sets HttpResponseMessage associated with the call if the call completed, otherwise null.
        /// </summary>
        public HttpResponseMessage HttpResponse { get; set; }

        /// <summary>
        /// Gets or sets exception that occurred while sending the HttpRequestMessage.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets dateTime the moment the request was sent.
        /// </summary>
        public DateTime StartedUtc { get; set; }

        /// <summary>
        /// Gets or sets dateTime the moment a response was received.
        /// </summary>
        public DateTime? EndedUtc { get; set; }

        /// <summary>
        /// Gets total duration of the call if it completed, otherwise null.
        /// </summary>
        public TimeSpan? Duration => EndedUtc - StartedUtc;

        /// <summary>
        /// Gets a value indicating whether response was received, regardless of whether it is an error status.
        /// </summary>
        public bool Completed => HttpResponse != null;

        /// <summary>
        /// Gets a value indicating whether a response with a successful HTTP status was received.
        /// </summary>
        public bool Succeeded => Completed && HttpResponse.IsSuccessStatusCode;

        /// <summary>
        /// Gets httpStatusCode of the response if the call completed, otherwise null.
        /// </summary>
        public HttpStatusCode? HttpStatus => Completed ? (HttpStatusCode?)HttpResponse.StatusCode : null;

        /// <summary>
        /// Returns the verb and absolute URI associated with this call.
        /// </summary>
        /// <returns> method and url <langword>string</langword> </returns>
        public override string ToString()
        {
            return $"{HttpRequest.Method:U} {ZenRequest.Url.ToString()}";
        }
    }
}
