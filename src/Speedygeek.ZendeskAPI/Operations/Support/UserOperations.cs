// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
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
        public Task<UserResponse> GetAsync(long userId, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/{userId}.json", sideload);
            return SendAsync<UserResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetAllAsync(PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam("users.json", sideload, pageParameters);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetInRolesAsync(UserRoles roles, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            if (roles.HasFlag(UserRoles.None))
            {
                roles &= ~UserRoles.None;
            }

            var rolesQuery = roles.ToLowerInvariantString();
            rolesQuery = roles.IsSingleFlag() ? $"role={rolesQuery}" : $"role[]={rolesQuery.Replace(",", "&role[]=")}";

            var requestUri = GetSideLoadParam($"users.json?{rolesQuery}", sideload, pageParameters);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetInCustomRoleAsync(long roleId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users.json?permission_set={roleId}", sideload, pageParameters);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetByGroupAsync(long groupId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"groups/{groupId}/users.json", sideload, pageParameters);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetByOrganizationAsync(long organizationId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"organizations/{organizationId}/users.json", sideload, pageParameters);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetManyAsync(IList<long> ids, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            var requestUri = GetSideLoadParam($"users/show_many.json?ids={ids.ToCsv()}", sideload);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetManyByExternalIdsAsync(IList<string> externalIds, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default)
        {
            if (externalIds.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(externalIds));
            }

            var requestUri = GetSideLoadParam($"users/show_many.json?external_ids={externalIds.ToCsv()}", sideload);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserRelatedResponse> GetRelatedInfoAsync(long userId, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserRelatedResponse>(HttpMethod.Get, $"users/{userId}/related.json", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserResponse>(HttpMethod.Post, $"users.json", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> CreateManyAsync(IList<User> users, CancellationToken cancellationToken = default)
        {
            return SendAsync<JobStatusResponse>(HttpMethod.Post, $"users/create_many.json", new { users }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> CreateOrUpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserResponse>(HttpMethod.Post, $"users/create_or_update.json", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> CreateOrUpdateManyAsync(IList<User> users, CancellationToken cancellationToken = default)
        {
            if (users.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(users));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Post, $"users/create_or_update_many.json", new { users }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> MergeAsync(long fromId, long toId, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserResponse>(HttpMethod.Put, $"users/{fromId}/merge.json", new { user = new { id = toId } }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserResponse>(HttpMethod.Put, $"users/{user.Id}.json", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBatchAsync(IList<User> users, CancellationToken cancellationToken = default)
        {
            if (users.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(users));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Put, $"users/update_many.json", new { users }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBulkAsync(User user, IList<long> ids, CancellationToken cancellationToken = default)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (ids.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(ids));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Put, $"users/update_many.json?ids={ids.ToCsv()}", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<JobStatusResponse> UpdateBulkAsync(User user, IList<string> externalIds, CancellationToken cancellationToken = default)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (externalIds.Count > 100)
            {
                throw new ArgumentException(Constants.MaxListSizeMessage, nameof(externalIds));
            }

            return SendAsync<JobStatusResponse>(HttpMethod.Put, $"users/update_many.json?external_ids={externalIds.ToCsv()}", new { user }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> DeleteBulkAsync(IList<long> ids, CancellationToken cancellationToken = default)
        {
            return SendAyncAsync(
                HttpMethod.Delete,
                $"users/destroy_many.json?ids={ids.ToCsv()}",
                IsStatus200OK,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> DeleteBulkAsync(IList<string> externalIds, CancellationToken cancellationToken = default)
        {
            return SendAyncAsync(
                HttpMethod.Delete,
                $"users/destroy_many.json?external_ids={externalIds.ToCsv()}",
                IsStatus200OK,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<UserResponse>(HttpMethod.Delete, $"users/{id}.json", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> SearchAsync(string query, PageParameters pageParameters = default, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"users/search.json?query={query}", UserSideloads.None, pageParameters);
            return SendAsync<UserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserResponse> SetPhotoAsync(long id, ZenFile photo, CancellationToken cancellationToken = default)
        {
            var formData = new Dictionary<string, object> { { "user[photo][uploaded_data]", photo } };
            return SendAsync<UserResponse>(HttpMethod.Put, $"users/{id}.json", formData, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedUserListResponse> GetDeletedAsync(PageParameters pageParameters = default, CancellationToken cancellationToken = default)
        {
            var requestUri = GetSideLoadParam($"deleted_users.json", UserSideloads.None, pageParameters);
            return SendAsync<DeletedUserListResponse>(HttpMethod.Get, requestUri, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedUserResponse> GetDeletedAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<DeletedUserResponse>(HttpMethod.Get, $"deleted_users/{id}.json", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeletedUserResponse> PermanentlyDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return SendAsync<DeletedUserResponse>(HttpMethod.Delete, $"deleted_users/{id}.json", cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<UserListResponse> GetNextPageAsync(Uri nextPage, CancellationToken cancellationToken = default)
        {
            if (nextPage is null)
            {
                throw new ArgumentNullException(nameof(nextPage));
            }

            // var pageUri = new Uri(nextPage);
            return SendAsync<UserListResponse>(HttpMethod.Get, nextPage.PathAndQuery, cancellationToken: cancellationToken);
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
