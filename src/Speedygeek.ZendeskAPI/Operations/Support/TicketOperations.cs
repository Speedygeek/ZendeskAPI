// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models.Support;

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
        public Task<TicketResponse> GetTicket(long id)
        {
            return SendAync<TicketResponse>(HttpMethod.Get, $"tickets/{id}.json");
        }

        /// <inheritdoc />
        public Task<TicketResponse> Create(Ticket ticket)
        {
            return SendAync<TicketResponse>(HttpMethod.Post, "tickets.json", new { ticket });
        }
    }
}
