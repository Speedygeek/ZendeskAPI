// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Operations that can be done with <see cref="Ticket"/>
    /// </summary>
    public class TicketOperations : BaseOperations, ITicketOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketOperations"/> class.
        /// </summary>
        /// <param name="restClient">client used to make HTTP calls with</param>
        public TicketOperations(IRESTClient restClient)
            : base(restClient)
        {
        }

        /// <inheritdoc />
        public Task<TicketResponse> GetTicket(long id, TicketSideload sideload = TicketSideload.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets/{id}.json", sideload);
            return SendAync<TicketResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketResponse> Create(Ticket ticket, CancellationToken cancellationToken = default)
        {
            return SendAync<TicketResponse>(HttpMethod.Post, "tickets.json", new { ticket }, cancellationToken);
        }

        private string GetSideLoadParam(string requestUri, TicketSideload options)
        {
            if (options != TicketSideload.None)
            {
                if (options.HasFlag(TicketSideload.None))
                {
                    options &= ~TicketSideload.None;
                }

                var sideLoads = options.ToString().ToLowerInvariant();
                requestUri.BuildQueryString(new Dictionary<string, string> { { "include", sideLoads } });
            }

            return requestUri;
        }
    }
}
