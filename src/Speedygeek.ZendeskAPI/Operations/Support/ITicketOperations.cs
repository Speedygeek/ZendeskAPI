// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
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
        Task<TicketResponse> GetAsync(long id, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Create a new Ticket
        /// </summary>
        /// <param name="ticket"> <see cref="Ticket"/> to save</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>created <see cref="Ticket"/></returns>
        Task<TicketResponse> CreateAsync(Ticket ticket, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Create a new Tickets
        /// </summary>
        /// <param name="tickets">Tickets to save</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>created <see cref="Ticket"/></returns>
        Task<JobStatusResponse> CreateManyAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates <see cref="Ticket" />
        /// </summary>
        /// <param name="ticket">the ticket to update</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="TicketResponse"/></returns>
        Task<TicketResponse> UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a list of tickets
        /// Note: Max size of 100 tickets
        /// </summary>
        /// <param name="tickets">list of ticket to update</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="JobStatusResponse"/></returns>
        Task<JobStatusResponse> UpdateBatchAsync(IList<Ticket> tickets, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a list of tickets
        /// Note: Max size of 100 tickets
        /// </summary>
        /// <param name="ticket">change state for update</param>
        /// <param name="ids">list of ticket ids to update</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="JobStatusResponse"/></returns>
        Task<JobStatusResponse> UpdateBulkAsync(Ticket ticket, IList<long> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the given Id (soft)
        /// </summary>
        /// <param name="id"> Id of the <see cref="Ticket"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a true if the Delete is successful </returns>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Permanently Deletes the given Id
        ///  Must first soft delete ticket <see cref="DeleteAsync(long, CancellationToken)"/>
        ///  This operation can't be undone.
        /// </summary>
        /// <param name="id"> Id of the <see cref="Ticket"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="JobStatusResponse"/> </returns>
        Task<JobStatusResponse> DeletePermanentlyAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Permanently Deletes the given list of Ids
        /// Must first soft delete tickets <see cref="DeleteBulkAsync(IList{long}, CancellationToken)"/>
        /// This operation can't be undone.
        /// </summary>
        /// <param name="ids"> list of ticket Ids to delete</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="JobStatusResponse"/> </returns>
        Task<JobStatusResponse> DeletePermanentlyBulkAsync(IList<long> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the given list of Ids (soft)
        /// </summary>
        /// <param name="ids"> list of ticket Ids to delete</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="JobStatusResponse"/> </returns>
        Task<JobStatusResponse> DeleteBulkAsync(IList<long> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all deleted (and not yet archived) tickets
        /// that have not yet been scrubbed in the past 30 days
        /// </summary>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<DeletedTicketListResponse> GetDeletedAsync(TicketPageParams pageParameters = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restore a previously deleted ticket
        /// </summary>
        /// <param name="id"> Id of the <see cref="Ticket"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a true if the Restore is successful </returns>
        Task<bool> RestoreAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restore previously deleted tickets in bulk
        /// </summary>
        /// <param name="ids">List of Ids to restore</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a true if the Restore is successful </returns>
        Task<bool> RestoreBulkAsync(IList<long> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all <see cref="Ticket"/>
        /// </summary>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetAllAsync(TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of <see cref="Ticket"/> given ids
        /// </summary>
        /// <param name="ids">list of ticket Ids to load</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetManyAsync(IList<long> ids, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of Recent <see cref="Ticket"/>
        /// Note: lists tickets that the requesting agent recently viewed in the agent interface,
        /// not recently created or updated tickets (unless by the agent recently in the agent interface).
        /// </summary>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetRecentAsync(TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve tickets for a given organization
        /// </summary>
        /// <param name="orgId">Id of the Organization</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name = "sideload" >side - load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetByOrganizationAsync(long orgId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve tickets for a given requesting user
        /// </summary>
        /// <param name="userId">Id of the requesting user</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name = "sideload" >side - load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetByRequestedUserAsync(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve tickets for a given assigned user
        /// </summary>
        /// <param name="userId">Id of the assigned user</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name = "sideload" >side - load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetByAssignedUserAsync(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve tickets for a given carbon copied user
        /// </summary>
        /// <param name="userId">Id of the carbon copied user</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name = "sideload" >side - load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetByCarbonCopiedUserAsync(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve tickets for a given external Id
        /// Note: External ids don't have to be unique for each ticket. As a result,
        /// the request may return multiple tickets with the same external id.
        /// </summary>
        /// <param name="externalId">External Id</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name = "sideload" >side - load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetByExternalIdAsync(string externalId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the given Id as spam
        /// </summary>
        /// <param name="id"> Id of the <see cref="Ticket"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a true if mark as spam is successful </returns>
        Task<bool> MarkAsSpamAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the given list of Ids as spam
        /// </summary>
        /// <param name="ids"> List of Ids to mark as spam</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="JobStatusResponse"/> </returns>
        Task<JobStatusResponse> MarkAsSpamAsync(IList<long> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Merge Tickets in to Target
        /// </summary>
        /// <param name="targetId">Merges one or more tickets into the ticket with the specified id</param>
        /// <param name="sourceIds">Ids of tickets to merge into the target ticket</param>
        /// <param name="targetComment">Private comment to add to the target ticket</param>
        /// <param name="sourceComment">Private comment to add to the source tickets</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="JobStatusResponse"/> </returns>
        Task<JobStatusResponse> MergeAsync(long targetId, IList<long> sourceIds, string targetComment = "", string sourceComment = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// Collaborators for a Ticket
        /// </summary>
        /// <param name="id">Id for ticket to load</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Returns a <see cref="UserListResponse"/></returns>
        Task<UserListResponse> GetCollaboratorsAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Followers for a Ticket
        /// </summary>
        /// <param name="id">Id for ticket to load</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Returns a <see cref="UserListResponse"/></returns>
        Task<UserListResponse> GetFollowersAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Email CCs for a Ticket
        /// </summary>
        /// <param name="id">Id for ticket to load</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Returns a <see cref="UserListResponse"/></returns>
        Task<UserListResponse> GetEmailCCsAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Listing Ticket Incidents
        /// </summary>
        /// <param name="id">id of ticket to get incidents for</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetIncidentsAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// will load the page for a list of <see cref="Ticket"/>
        /// </summary>
        /// <param name="page">URL of the page</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="TicketListResponse"/> </returns>
        Task<TicketListResponse> GetPageAsync(Uri page, CancellationToken cancellationToken = default);
    }
}
