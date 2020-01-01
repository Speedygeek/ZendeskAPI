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
        public Task<JobStatusResponse> CreateMany(IEnumerable<Ticket> tickets, CancellationToken cancellationToken = default)
        {
            return SendAync<JobStatusResponse>(HttpMethod.Post, "tickets/create_many.json", new { tickets }, cancellationToken);
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
        public Task<JobStatusResponse> UpdateBatch(IList<Ticket> tickets, CancellationToken cancellationToken = default)
        {
            if (tickets is null)
            {
                throw new ArgumentNullException(nameof(tickets));
            }

            if (tickets.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(tickets));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Put, "tickets/update_many.json", new { tickets }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBulk(Ticket ticket, IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Put, $"tickets/update_many.json?ids={ids.ToCsv()}", new { ticket }, cancellationToken);
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
        public Task<JobStatusResponse> DeletePermanently(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<JobStatusResponse>(HttpMethod.Delete, $"deleted_tickets/{id}.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> DeletePermanentlyBulk(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Delete, $"deleted_tickets/destroy_many.json?ids={ids.ToCsv()}", cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> DeleteBulk(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Delete, $"tickets/destroy_many.json?ids={ids.ToCsv()}", cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedTicketListResponse> GetDeleted(TicketPageParams pageParameters = default, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam("deleted_tickets.json", TicketSideloads.None, pageParameters);
            return SendAync<DeletedTicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> Restore(long id, CancellationToken cancellationToken = default)
        {
            return SendAync(
                HttpMethod.Put,
                $"deleted_tickets/{id}/restore.json",
                (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.OK); },
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> RestoreBulk(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAync(
                HttpMethod.Put,
                $"deleted_tickets/restore_many.json?ids={ids.ToCsv()}",
                (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.OK); },
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetAll(TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam("tickets.json", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetMany(IList<long> ids, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            var requestUri = GetSideLoadParam($"tickets/show_many.json?ids={ids.ToCsv()}", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetRecent(TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets/recent.json", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByOrganization(long orgId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"organizations/{orgId}/tickets.json", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByRequestedUser(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}/tickets/requested.json", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByAssignedUser(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}/tickets/assigned.json", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByCarbonCopiedUser(long userId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}/tickets/ccd.json", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetByExternalId(string externalId, TicketPageParams pageParameters = default, TicketSideloads sideload = TicketSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"tickets.json?external_id={externalId}", sideload, pageParameters);
            return SendAync<TicketListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> MarkAsSpam(long id, CancellationToken cancellationToken = default)
        {
            return SendAync(
                 HttpMethod.Put,
                 $"tickets/{id}/mark_as_spam.json",
                 (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.OK); },
                 cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> MarkAsSpam(IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Put, $"tickets/mark_many_as_spam.json?ids={ids.ToCsv()}", cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> Merge(long targetId, IList<long> sourceIds, string targetComment = "", string sourceComment = "", CancellationToken cancellationToken = default)
        {
            return SendAync<JobStatusResponse>(HttpMethod.Post, $"tickets/{targetId}/merge.json", new { ids = sourceIds, targetComment, sourceComment }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetCollaborators(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<UserListResponse>(HttpMethod.Get, $"tickets/{id}/collaborators.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> Getfollowers(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<UserListResponse>(HttpMethod.Get, $"tickets/{id}/followers.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetEmailCCs(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<UserListResponse>(HttpMethod.Get, $"tickets/{id}/email_ccs.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetIncidents(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<TicketListResponse>(HttpMethod.Get, $"tickets/{id}/incidents.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<TicketListResponse> GetNextPage(Uri nextPage, CancellationToken cancellationToken = default)
        {
            if (nextPage is null)
            {
                throw new ArgumentNullException(nameof(nextPage));
            }

            return SendAync<TicketListResponse>(HttpMethod.Get, nextPage.PathAndQuery, cancellationToken);
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
