// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Operations that can be done with <see cref="Ticket"/>
    /// </summary>
    public interface ITicketOperations
    {
        /// <summary>
        /// A single <see cref="Ticket"/>
        /// </summary>
        /// <param name="id">Id of the <see cref="Ticket"/> to load</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a number of <see cref="Ticket"/> properties though not the ticket comments.</returns>
        Task<TicketResponse> Get(long id, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Create a new Ticket
        /// </summary>
        /// <param name="ticket"> <see cref="Ticket"/> to save</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>created <see cref="Ticket"/></returns>
        Task<TicketResponse> Create(Ticket ticket, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates <see cref="Ticket" />
        /// </summary>
        /// <param name="ticket">the ticket to update</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="TicketResponse"/></returns>
        Task<TicketResponse> Update(Ticket ticket, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the given Id
        /// </summary>
        /// <param name="id"> Id of the <see cref="Ticket"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a true if the Delete is successful </returns>
        Task<bool> Delete(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all <see cref="Ticket"/>
        /// </summary>
        /// <param name="sideload"> side-load options</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetAll(TicketSideloads sideload = TicketSideloads.None, TicketPageParams pageParameters = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve tickets for a given organization
        /// </summary>
        /// <param name="orgId">Id of the Organization </param>
        /// <param name = "sideload" >side - load options</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetByOrganization(long orgId, TicketSideloads sideload = TicketSideloads.None, TicketPageParams pageParameters = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// will load the next page for a list of <see cref="Ticket"/>
        /// </summary>
        /// <param name="nextPage">URL of the next page</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetNextPage(Uri nextPage, CancellationToken cancellationToken = default);
    }
}
