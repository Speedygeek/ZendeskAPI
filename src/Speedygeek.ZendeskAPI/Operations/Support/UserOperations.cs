// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Operations that can be done with <see cref="User"/>
    /// </summary>
    public class UserOperations : BaseOperations, IUserOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserOperations"/> class.
        /// </summary>
        /// <param name="restClient">client used to make HTTP calls with</param>
        public UserOperations(IRESTClient restClient)
            : base(restClient)
        {
        }

        /// <inheritdoc />
        public Task<UserResponse> Get(long userId, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}.json", sideload);
            return SendAync<UserResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetAll(PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam("users.json", sideload, pageParameters);
            return SendAync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetInRoles(UserRoles roles, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            if (roles.HasFlag(UserRoles.None))
            {
                roles &= ~UserRoles.None;
            }

            var rolesQuery = roles.ToLowerInvariantString();
            rolesQuery = roles.IsSingleFlag() ? $"role={rolesQuery}" : $"role[]={rolesQuery.Replace(",", "&role[]=")}";

            var requestUri = GetSideLoadParam($"users.json?{rolesQuery}", sideload, pageParameters);
            return SendAync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetInCustomRole(long roleId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users.json?permission_set={roleId}", sideload, pageParameters);
            return SendAync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetByGroup(long groupId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"groups/{groupId}/users.json", sideload, pageParameters);
            return SendAync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetByOrganization(long organizationId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"organizations/{organizationId}/users.json", sideload, pageParameters);
            return SendAync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> GetMany(IList<long> ids, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            var requestUri = GetSideLoadParam($"users/show_many.json?ids={ids.ToCsv()}", sideload);
            return SendAync<UserResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> GetManyByExternalIds(IList<string> externalIds, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            if (externalIds.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(externalIds));
            }

            var requestUri = GetSideLoadParam($"users/show_many.json?external_ids={externalIds.ToCsv()}", sideload);
            return SendAync<UserResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserRelatedResponse> GetRelatedInfo(long userId, CancellationToken cancellationToken = default)
        {
            return SendAync<UserRelatedResponse>(HttpMethod.Get, $"users/{userId}/related.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> Create(User user, CancellationToken cancellationToken = default)
        {
            return SendAync<UserResponse>(HttpMethod.Post, $"users.json", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> CreateMany(IList<User> users, CancellationToken cancellationToken = default)
        {
            return SendAync<JobStatusResponse>(HttpMethod.Post, $"users/create_many.json", new { users }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> CreateOrUpdate(User user, CancellationToken cancellationToken = default)
        {
            return SendAync<UserResponse>(HttpMethod.Post, $"users/create_or_update.json", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> CreateOrUpdateMany(IList<User> users, CancellationToken cancellationToken = default)
        {
            if (users.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(users));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Post, $"users/create_or_update_many.json", new { users }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> Merge(long fromId, long toId, CancellationToken cancellationToken = default)
        {
            return SendAync<UserResponse>(HttpMethod.Put, $"users/{fromId}/merge.json", new { user = new { id = toId } }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> Update(User user, CancellationToken cancellationToken = default)
        {
            return SendAync<UserResponse>(HttpMethod.Put, $"users/{user.Id}.json", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBatch(IList<User> users, CancellationToken cancellationToken = default)
        {
            if (users.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(users));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Put, $"users/update_many.json", new { users }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBulk(User user, IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Put, $"users/update_many.json?ids={ids.ToCsv()}", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBulk(User user, IList<string> externalIds, CancellationToken cancellationToken = default)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (externalIds.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(externalIds));
            }

            return SendAync<JobStatusResponse>(HttpMethod.Put, $"users/update_many.json?external_ids={externalIds.ToCsv()}", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> DeleteBulk(IList<long> ids, CancellationToken cancellationToken = default)
        {
            return SendAync(
                HttpMethod.Delete,
                $"users/destroy_many.json?ids={ids.ToCsv()}",
                (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.OK); },
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> DeleteBulk(IList<string> externalIds, CancellationToken cancellationToken = default)
        {
            return SendAync(
                HttpMethod.Delete,
                $"users/destroy_many.json?external_ids={externalIds.ToCsv()}",
                (HttpResponseMessage resp) => { return Task.FromResult(resp.StatusCode == HttpStatusCode.OK); },
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> Delete(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<UserResponse>(HttpMethod.Delete, $"users/{id}.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> Search(string query, PageParameters pageParameters = default, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/search.json?query={query}", UserSideloads.None, pageParameters);
            return SendAync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> SetPhoto(long id, ZenFile photo, CancellationToken cancellationToken = default)
        {
            var formData = new Dictionary<string, object> { { "user[photo][uploaded_data]", photo } };
            return SendAync<UserResponse>(HttpMethod.Put, $"users/{id}.json", formData, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedUserListResponse> GetDeleted(PageParameters pageParameters = default, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"deleted_users.json", UserSideloads.None, pageParameters);
            return SendAync<DeletedUserListResponse>(HttpMethod.Get, requestUri, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedUserResponse> GetDeleted(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<DeletedUserResponse>(HttpMethod.Get, $"deleted_users/{id}.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedUserResponse> PermanentlyDelete(long id, CancellationToken cancellationToken = default)
        {
            return SendAync<DeletedUserResponse>(HttpMethod.Delete, $"deleted_users/{id}.json", cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetNextPage(Uri nextPage, CancellationToken cancellationToken = default)
        {
            if (nextPage is null)
            {
                throw new ArgumentNullException(nameof(nextPage));
            }

            return SendAync<UserListResponse>(HttpMethod.Get, nextPage.PathAndQuery, cancellationToken);
        }

        private static string GetSideLoadParam(string requestSuffix, UserSideloads options, PageParameters pageParameters = default)
        {
            var queryParams = new Dictionary<string, string>();

            if (options != UserSideloads.None)
            {
                if (options.HasFlag(UserSideloads.None))
                {
                    options &= ~UserSideloads.None;
                }

                var sideLoads = options.ToLowerInvariantString();

                queryParams.Add("include", sideLoads.Replace(" ", string.Empty));
            }

            if (pageParameters != null)
            {
                queryParams.Merge(pageParameters.ToParameters());
            }

            return requestSuffix.BuildQueryString(queryParams);
        }
    }
}
