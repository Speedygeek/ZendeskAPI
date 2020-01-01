// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Zendesk Support has three types of users: end-users (your customers), agents, and administrators.
    /// </summary>
    public class User : ZenEntity
    {
        /// <summary>
        /// The user's primary email address.
        /// Writable on create only. On update, a secondary email is added.
        /// To change the primary email address <see href="https://developer.zendesk.com/rest_api/docs/support/user_identities#make-identity-primary"/>
        /// </summary>
        ///
        public string Email { get; set; }

        /// <summary>
        /// The user's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <see langword="false"/> if the user has been deleted
        /// </summary>
        public bool Active { get; }

        /// <summary>
        /// An alias displayed to end users
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Whether or not the user is a chat-only agent
        /// </summary>
        public bool ChatOnly { get; set; }

        /// <summary>
        /// A custom role if the user is an agent on the Enterprise plan
        /// </summary>
        public long CustomRoleId { get; set; }

        /// <summary>
        /// The user's role id. 0 for custom agents, 1 for light agent and 2 for chat agent
        /// </summary>
        public long RoleType { get; set; }

        /// <summary>
        /// Any details you want to store about the user, such as an address
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// A unique identifier from another system. The API treats the id as case insensitive.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// The last time the user signed in to Zendesk Support
        /// </summary>
        public DateTimeOffset LastLoginAt { get; }

        /// <summary>
        /// The user's locale. A BCP-47 compliant tag for the locale.
        /// </summary>
        /// <remarks><para>If both "locale" and "locale_id" are present on create or update, "locale_id" is ignored and only "locale" is used.</para></remarks>
        public string Locale { get; set; }

        /// <summary>
        /// The user's language identifier
        /// </summary>
        public long LocaleId { get; set; }

        /// <summary>
        /// Designates whether the user has forum moderation capabilities
        /// </summary>
        public bool Moderator { get; set; }

        /// <summary>
        /// Any notes you want to store about the user
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// <see langword="true"/> if the user can only create private comments
        /// </summary>
        public bool OnlyPrivateComments { get; set; }

        /// <summary>
        /// The id of the user's default organization
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// The id of the user's default group.
        /// Can only be set on create, not on update
        /// </summary>
        public long DefaultGroupId { get; set; }

        /// <summary>
        /// The user's primary phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Whether the phone number is shared or not.
        /// </summary>
        public bool SharedPhoneNumber { get; }

        /// <summary>
        /// The user's profile picture represented as an <see cref="Attachment"/> object
        /// </summary>
        public Attachment Photo { get; set; }

        /// <summary>
        /// If the agent has any restrictions; <see langword="false"/> for administrators
        /// and unrestricted agents, <see langword="true"/> for other agents
        /// </summary>
        public bool RestrictedAgent { get; set; }

        /// <summary>
        /// The user's role.
        /// </summary>
        public UserRoles Role { get; set; }

        /// <summary>
        /// If the user is shared from a different Zendesk Support instance.
        /// Ticket sharing accounts only
        /// </summary>
        public bool Shared { get; }

        /// <summary>
        /// If the user is shared from a different Zendesk Support instance.
        /// Ticket sharing accounts only
        /// </summary>
        public bool SharedAgent { get; set; }

        /// <summary>
        /// The user's tags.
        /// Only present if your account has user tagging enabled
        /// </summary>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Specifies which tickets the user has access to.
        /// </summary>
        public UserTicketRestriction TicketRestriction { get; set; }

        /// <summary>
        /// The user's time zone.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// If two factor authentication is enabled.
        /// </summary>
        public bool TwoFactorAuthEnabled { get; }

        /// <summary>
        /// Values of custom fields in the user's profile.
        /// </summary>
        public dynamic UserFields { get; set; }

        /// <summary>
        /// The user's primary identity is verified or not.
        /// </summary>
        public bool Verified { get; set; }

        /// <summary>
        /// Whether or not the user can access the CSV report
        /// on the Search tab of the Reporting page in the
        /// Support admin interface.
        /// </summary>
        public bool ReportCsv { get; }

        /// <summary>
        /// You can update a user's profile image by referring to an image hosted on a different website.
        /// This may take a few minutes to process.
        /// Update only
        /// </summary>
        public string RemotePhotoUrl { get; set; }
    }
}
