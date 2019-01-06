// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Dynamic;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Http;

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// An exception that is thrown when an HTTP call fails, including when the response
    /// indicates an unsuccessful HTTP status code.
    /// </summary>
    public class ZenException : Exception
    {
        private readonly string _capturedResponseBody;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZenException"/> class.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ZenException(HttpCall call, string message, Exception inner)
            : this(call, message, null, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZenException"/> class.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <param name="message">The message.</param>
        /// <param name="capturedResponseBody">The captured response body, if available.</param>
        /// <param name="inner">The inner.</param>
        public ZenException(HttpCall call, string message, string capturedResponseBody, Exception inner)
            : base(message, inner)
        {
            Call = call;
            _capturedResponseBody = capturedResponseBody;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZenException"/> class.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <param name="inner">The inner.</param>
        public ZenException(HttpCall call, Exception inner)
            : this(call, BuildMessage(call, inner), inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZenException"/> class.
        /// </summary>
        /// <param name="call">The call.</param>
        public ZenException(HttpCall call)
            : this(call, BuildMessage(call, null), null)
        {
        }

        /// <summary>
        /// Gets an object containing details about the failed HTTP call
        /// </summary>
        public HttpCall Call { get; }

        private static string BuildMessage(HttpCall call, Exception inner)
        {
            return
                (call.HttpResponse != null && !call.Succeeded) ?
                $"Call failed with status code {(int)call.HttpResponse.StatusCode} ({call.HttpResponse.ReasonPhrase}): {call}" :
                $"Call failed. {inner?.Message} {call}";
        }

        /// <summary>
        /// Gets the response body of the failed call.
        /// </summary>
        /// <returns>A task whose result is the string contents of the response body.</returns>
        public Task<string> GetResponseStringAsync()
        {
            if (_capturedResponseBody != null)
            {
                return Task.FromResult(_capturedResponseBody);
            }

            var task = Call?.HttpResponse?.Content?.ReadAsStringAsync();
            return task ?? Task.FromResult((string)null);
        }

        /// <summary>
        /// Deserializes the JSON response body to an object of the given type.
        /// </summary>
        /// <typeparam name="T">A type whose structure matches the expected JSON response.</typeparam>
        /// <returns>A task whose result is an object containing data in the response body.</returns>
        public async Task<T> GetResponseJsonAsync<T>()
        {
            var ser = Call.ZenRequest?.Settings?.JsonSerializer;
            if (ser == null)
            {
                return default;
            }

            if (_capturedResponseBody != null)
            {
                return ser.Deserialize<T>(_capturedResponseBody);
            }

            var task = Call?.HttpResponse?.Content?.ReadAsStreamAsync();
            if (task == null)
            {
                return default;
            }

            return ser.Deserialize<T>(await task.ConfigureAwait(false));
        }

        /// <summary>
        /// Deserializes the JSON response body to a dynamic object.
        /// </summary>
        /// <returns>A task whose result is an object containing data in the response body.</returns>
        public async Task<dynamic> GetResponseJsonAsync() => await GetResponseJsonAsync<ExpandoObject>().ConfigureAwait(false);
    }
}
