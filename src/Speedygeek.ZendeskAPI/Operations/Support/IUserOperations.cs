// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Operations that can be done with <see cref="User"/>
    /// </summary>
    public interface IUserOperations
    {
        /// <summary>
        /// Get a <see cref="User"/>
        /// </summary>
        /// <param name="userId">id of the user to request</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserResponse> Get(long userId, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all <see cref="User"/>
        /// </summary>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserListResponse> GetAll(PageParameters pageParameters = null, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all <see cref="User"/> in one or more roles
        /// </summary>
        /// <param name="roles">roles to load</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        /// <see href="https://developer.zendesk.com/rest_api/docs/support/users#filters"/>
        Task<UserListResponse> GetInRoles(UserRoles roles, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all <see cref="User"/> in custom role
        /// You can only filter by one role id per request
        /// For the Enterprise plan only
        /// </summary>
        /// <param name="roleId">id of custom role</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        /// <see href="https://developer.zendesk.com/rest_api/docs/support/users#filters"/>
        Task<UserListResponse> GetInCustomRole(long roleId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all <see cref="User"/> by group
        /// </summary>
        /// <param name="groupId">group id</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserListResponse> GetByGroup(long groupId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of all <see cref="User"/> by organization
        /// </summary>
        /// <param name="organizationId">organization id</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserListResponse> GetByOrganization(long organizationId, PageParameters pageParameters = default, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of requested <see cref="User"/>
        /// </summary>
        /// <param name="ids">list of User ids to load</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserResponse> GetMany(IList<long> ids, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of requested <see cref="User"/>
        /// </summary>
        /// <param name="externalIds">list of User external ids to load</param>
        /// <param name="sideload"> side-load options</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserResponse> GetManyByExternalIds(IList<string> externalIds, UserSideloads sideload = UserSideloads.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="UserRelatedResponse" />
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserRelatedResponse"/> </returns>
        Task<UserRelatedResponse> GetRelatedInfo(long userId, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Create a new user
        /// </summary>
        /// <param name="user"> <see cref="User"/> to save</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="UserResponse"/></returns>
        Task<UserResponse> Create(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create new users
        /// Note: Access to this endpoint is limited. You may not be able to use it to bulk-import users or organizations.
        /// If you're unable, a 403 Forbidden error is returned.
        /// If you need access and can't use this endpoint, contact us at support@zendesk.com for assistance.
        /// </summary>
        /// <param name="users">list of <see cref="User"/> to create</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="JobStatusResponse"/></returns>
        Task<JobStatusResponse> CreateMany(IList<User> users, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Create or update user
        /// </summary>
        /// <param name="user"> <see cref="User"/> to save</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="UserResponse"/></returns>
        Task<UserResponse> CreateOrUpdate(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create or update many users
        /// </summary>
        /// <param name="users">list of users to update</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="JobStatusResponse"/></returns>
        Task<JobStatusResponse> CreateOrUpdateMany(IList<User> users, CancellationToken cancellationToken = default);

        /// <summary>
        /// Can merge two end users
        /// Agents and administrators cannot be merged.
        /// </summary>
        /// <param name="fromId">user to merge (will be removed)</param>
        /// <param name="toId">resulting user</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="UserResponse"/></returns>
        Task<UserResponse> Merge(long fromId, long toId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="user">the user to be updated</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="UserResponse"/></returns>
        Task<UserResponse> Update(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// batch update of users
        /// </summary>
        /// <param name="users">Users to update</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="JobStatusResponse"/></returns>
        Task<JobStatusResponse> UpdateBatch(IList<User> users, CancellationToken cancellationToken = default);

        /// <summary>
        /// Bulk update users by user id
        /// </summary>
        /// <param name="user">the template user</param>
        /// <param name="ids">list of user ids to apply template user</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="JobStatusResponse"/></returns>
        Task<JobStatusResponse> UpdateBulk(User user, IList<long> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Bulk update users by external id
        /// </summary>
        /// <param name="user">the template user</param>
        /// <param name="externalIds">list of user external ids</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="JobStatusResponse"/></returns>
        Task<JobStatusResponse> UpdateBulk(User user, IList<string> externalIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes many users by id
        /// </summary>
        /// <param name="ids">list of ids to delete</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> DeleteBulk(IList<long> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes many users by external ids
        /// </summary>
        /// <param name="externalIds">list of external ids to delete</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> DeleteBulk(IList<string> externalIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// deletes a the given user
        /// </summary>
        /// <param name="id">id of user to delete</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="UserResponse"/></returns>
        Task<UserResponse> Delete(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search for any user by query
        /// </summary>
        /// <param name="query">can specify a partial or full value of any user property,
        /// including name, email address, notes, or phone.</param>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserListResponse> Search(string query, PageParameters pageParameters = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets user photo
        /// </summary>
        /// <param name="id">id of user</param>
        /// <param name="photo">photo to save</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns><see cref="UserResponse"/></returns>
        Task<UserResponse> SetPhoto(long id, ZenFile photo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists deleted users, including permanently deleted users.
        /// For permanently deleted users, all properties containing personal data, such as email and phone, are null. The name property is "Permanently Deleted User".
        /// Returns a maximum of 100 users per page.
        /// </summary>
        /// <param name="pageParameters">Options about paging</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Returns a <see cref="DeletedUserListResponse"/></returns>
        Task<DeletedUserListResponse> GetDeleted(PageParameters pageParameters = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// These are users that have been deleted but not permanently yet.
        /// </summary>
        /// <param name="id">id of deleted user</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Returns a <see cref="DeletedUserResponse"/></returns>
        Task<DeletedUserResponse> GetDeleted(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Permanently Delete User
        /// You must delete the user first.
        /// Permanently deleting a user deletes all of their information.
        /// This information is not recoverable.
        /// </summary>
        /// <param name="id">user id to permanently delete</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Returns a <see cref="DeletedUserResponse"/></returns>
        Task<DeletedUserResponse> PermanentlyDelete(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// will load the next page for a list of <see cref="User"/>
        /// </summary>
        /// <param name="nextPage">URL of the next page</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns> Returns a <see cref="UserListResponse"/> </returns>
        Task<UserListResponse> GetNextPage(Uri nextPage, CancellationToken cancellationToken = default);
    }
}
