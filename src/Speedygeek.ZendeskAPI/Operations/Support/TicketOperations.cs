// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
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
        public Task<TicketResponse> Create(Ticket ticket, CancellationToken cancellationToken = default)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            return SendAync<TicketResponse>(HttpMethod.Post, "tickets.json", new { ticket }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketResponse> Get(long id, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets/{id}.json", sideload);
            return SendAync<TicketResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketResponse> Update(Ticket ticket, CancellationToken cancellationToken = default)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            return SendAync<TicketResponse>(HttpMethod.Post, $"tickets/{ticket.Id}.json", new { ticket }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> Delete(long id, CancellationToken cancellationToken = default)
        {
            return SendAync(
                HttpMethod.Delete,
                $"tickets/{id}.json",
                (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.NoContent || resp.StatusCode == HttpStatusCode.OK); },
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetAll(TicketSideloads sideload = TicketSideloads.None, TicketPageParams pageParameters = default, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets.json", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        private static string GetSideLoadParam(string requestSuffix, TicketSideloads options, TicketPageParams pageParameters = default)
        {
            var queryParams = new Dictionary<string, string>();

            if (options != TicketSideloads.None)
            {
                if (options.HasFlag(TicketSideloads.None))
                {
                    options &= ~TicketSideloads.None;
                }

                var sideLoads = options.ToString().ToLowerInvariant();

                queryParams.Add("include", sideLoads);
            }

            if (pageParameters != null)
            {
                queryParams.Merge(pageParameters.ToParameters());
            }

            return requestSuffix.BuildQueryString(queryParams);
        }
    }
}
