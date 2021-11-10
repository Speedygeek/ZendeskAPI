// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
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
        public Task<TicketResponse> CreateAsync(Ticket ticket, CancellationToken cancellationToken = default)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            return SendAsync<TicketResponse>(HttpMethod.Post, "tickets.json", new { ticket }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> CreateManyAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken = default)
        {
            return SendAsync<JobStatusResponse>(HttpMethod.Post, "tickets/create_many.json", new { tickets }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketResponse> GetAsync(long id, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets/{id}.json", sideload);
            return SendAsync<TicketResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketResponse> UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            return SendAsync<TicketResponse>(HttpMethod.Put, $"tickets/{ticket.Id}.json", new { ticket }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBatchAsync(IList<Ticket> tickets, CancellationToken cancellationToken = default)
        {
            if (tickets is null)
            {
                throw new ArgumentNullException(nameof(tickets));
            }

            if (tickets.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(tickets));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Put, "tickets/update_many.json", new { tickets }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBulkAsync(Ticket ticket, IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Put, $"tickets/update_many.json?ids={ids.ToCsv()}", new { ticket }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAyncAsync(
                HttpMethod.Delete,
                $"tickets/{id}.json",
                IsStatus204NoContentOr200OK,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> DeletePermanentlyAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<JobStatusResponse>(HttpMethod.Delete, $"deleted_tickets/{id}.json", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> DeletePermanentlyBulkAsync(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Delete, $"deleted_tickets/destroy_many.json?ids={ids.ToCsv()}", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> DeleteBulkAsync(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Delete, $"tickets/destroy_many.json?ids={ids.ToCsv()}", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedTicketListResponse> GetDeletedAsync(TicketPageParams pageParameters = default, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam("deleted_tickets.json", TicketSideloads.None, pageParameters);
            return SendAsync<DeletedTicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> RestoreAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAyncAsync(
                HttpMethod.Put,
                $"deleted_tickets/{id}/restore.json",
                IsStatus200OK,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> RestoreBulkAsync(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAyncAsync(
                HttpMethod.Put,
                $"deleted_tickets/restore_many.json?ids={ids.ToCsv()}",
                IsStatus200OK,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetAllAsync(TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam("tickets.json", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetManyAsync(IList<long> ids, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            var requestUri = GetSideLoadParam($"tickets/show_many.json?ids={ids.ToCsv()}", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetRecentAsync(TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets/recent.json", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByOrganizationAsync(long orgId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"organizations/{orgId}/tickets.json", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByRequestedUserAsync(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}/tickets/requested.json", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByAssignedUserAsync(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}/tickets/assigned.json", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByCarbonCopiedUserAsync(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}/tickets/ccd.json", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByExternalIdAsync(string externalId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets.json?external_id={externalId}", sideload, pageParameters);
            return SendAsync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> MarkAsSpamAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAyncAsync(
                 HttpMethod.Put,
                 $"tickets/{id}/mark_as_spam.json",
                 IsStatus200OK,
                 cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> MarkAsSpamAsync(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Put, $"tickets/mark_many_as_spam.json?ids={ids.ToCsv()}", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> MergeAsync(long targetId, IList<long> sourceIds, string targetComment = "", string sourceComment = "", CancellationToken cancellationToken = default)
        {
            return SendAsync<JobStatusResponse>(HttpMethod.Post, $"tickets/{targetId}/merge.json", new { ids = sourceIds, targetComment, sourceComment }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetCollaboratorsAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserListResponse>(HttpMethod.Get, $"tickets/{id}/collaborators.json", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetFollowersAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserListResponse>(HttpMethod.Get, $"tickets/{id}/followers", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetEmailCCsAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserListResponse>(HttpMethod.Get, $"tickets/{id}/email_ccs", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetIncidentsAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<TicketListResponse>(HttpMethod.Get, $"tickets/{id}/incidents.json", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetPageAsync(Uri page, CancellationToken cancellationToken = default)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page));
            }

           // var pageUri = new Uri(page);
            return SendAsync<TicketListResponse>(HttpMethod.Get, page.PathAndQuery, cancellationToken: cancellationToken);
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

                var sideLoads = options.ToLowerInvariantString();

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
