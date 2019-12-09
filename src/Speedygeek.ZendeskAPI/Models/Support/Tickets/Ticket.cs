// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Speedygeek.ZendeskAPI.Serialization.Converters;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Tickets are the means through which your end users (customers) communicate with agents in Zendesk Support.
    /// </summary>
    public class Ticket : ZenEntity
    {
        /// <summary>
        /// An id you can use to link to local records.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// The type of this ticket.
        /// </summary>
        public TicketType Type { get; set; }

        /// <summary>
        /// The value of the subject field for this ticket
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The dynamic content placeholder, if present, or the "subject" value, if not.
        /// <a href="https://developer.zendesk.com/rest_api/docs/support/dynamic_content.html">see Dynamic Content</a>
        /// </summary>
        public string RawSubject { get; set; }

        /// <summary>
        /// The first comment on the ticket.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The urgency with which the ticket should be addressed.
        /// </summary>
        public TicketPriority Priority { get; set; }

        /// <summary>
        /// The state of the ticket
        /// </summary>
        public TicketStatus Status { get; set; }

        /// <summary>
        /// The original recipient e-mail address of the ticket
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// The user who requested this ticket
        /// </summary>
        public long RequesterId { get; set; }

        /// <summary>
        /// The user who submitted the ticket.
        /// </summary>
        /// <remarks><para>The submitter always becomes the author of the first comment on the ticket</para></remarks>
        public long SubmitterId { get; set; }

        /// <summary>
        /// The agent currently assigned to the ticket
        /// </summary>
        public long AssigneeId { get; set; }

        /// <summary>
        /// The organization of the requester.
        /// You can only specify the ID of an organization associated with the requester.
        /// <see href="https://developer.zendesk.com/rest_api/docs/support/organization_memberships" />
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// The group this ticket is assigned to
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// The ids of users currently CC'ed on the ticket
        /// </summary>
        public IList<long> CollaboratorIds { get; set; }

        /// <summary>
        /// Update operations only
        /// Users to add as cc's when creating a ticket. See
        /// <seealso herf="https://developer.zendesk.com/rest_api/docs/support/tickets#setting-collaborators" />
        /// NOTE: that setting collaborators this way will completely ignore what's already set,
        /// so make sure to include existing collaborators in the array
        /// if you wish to retain these on the ticket.
        /// </summary>
        [JsonConverter(typeof(CollaboratorConverter))]
        public IList<Collaborator> Collaborators { get; set; }

        /// <summary>
        /// The ids of agents or end users currently CC'ed on the ticket.
        /// See <see href="https://support.zendesk.com/hc/en-us/articles/360020585233"/>
        /// </summary>
        public IList<string> EmailCcIds { get; set; }

        /// <summary>
        /// The ids of agents currently following the ticket.
        /// See <see href="https://support.zendesk.com/hc/en-us/articles/360020585233"/>
        /// </summary>
        public List<long> FollowerIds { get; set; }

        /// <summary>
        /// The topic this ticket originated from, if any
        /// </summary>
        public long ForumTopicId { get; set; }

        /// <summary>
        /// For tickets of type "incident", the ID of the problem the incident is linked to
        /// </summary>
        public long ProblemId { get; set; }

        /// <summary>
        /// Is true of this ticket has been marked as a problem, false otherwise
        /// </summary>
        public bool HasIncidents { get; }

        /// <summary>
        /// If this is a ticket of type "task" it has a due date.
        /// Due date format uses ISO 8601 format.
        /// </summary>
        public DateTimeOffset DueAt { get; set; }

        /// <summary>
        /// The tags applied to this ticket
        /// </summary>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Use to add tags in bulk updates
        /// <see href="https://developer.zendesk.com/rest_api/docs/support/tickets#updating-tag-lists"/>
        /// </summary>
        public IList<string> AdditionalTags { get; set; }

        /// <summary>
        /// Use to Remove tags in bulk updates
        /// <see href="https://developer.zendesk.com/rest_api/docs/support/tickets#updating-tag-lists"/>
        /// </summary>
        public IList<string> RemoveTags { get; set; }

        /// <summary>
        /// This object explains how the ticket was created
        /// </summary>
        public Via Via { get; set; }

        /// <summary>
        /// Custom fields for the ticket.
        /// </summary>
        public IList<CustomField> CustomFields { get; set; }

        /// <summary>
        /// The satisfaction rating of the ticket, if it exists
        /// </summary>
        public SatisfactionRating SatisfactionRating { get; set; }

        /// <summary>
        /// The ids of the sharing agreements used for this ticket.
        /// </summary>
        public IList<long> SharingAgreementIds { get; set; }

        /// <summary>
        /// The ids of the followup's created from this ticket.
        /// Ids are only visible once the ticket is closed
        /// </summary>
        public IList<long> FollowupIds { get; set; }

        /// <summary>
        /// Update operations only
        /// The id of a closed ticket when creating a follow-up ticket.
        /// <see href="https://developer.zendesk.com/rest_api/docs/support/tickets#creating-follow-up-tickets"/>
        /// </summary>
        public long ViaFollowupSourceId { get; set; }

        /// <summary>
        /// Update operations only
        /// List of macro IDs to be recorded in the ticket audit
        /// </summary>
        public IList<long> MacroIds { get; set; }

        /// <summary>
        /// Enterprise Accounts only.
        /// The id of the ticket form to render for the ticket
        /// </summary>
        public long TicketFormId { get; set; }

        /// <summary>
        /// Enterprise Accounts only.
        /// The id of the brand this ticket is associated with
        /// </summary>
        public long BrandId { get; set; }

        /// <summary>
        /// Is false if channelback is disabled, true otherwise.
        /// Only applicable for channels framework ticket
        /// </summary>
        public bool AllowChannelback { get; set; }

        /// <summary>
        /// When an agent responds, are they allowed to add attachments?
        /// Defaults to true
        /// </summary>
        public bool AllowAttachments { get; set; }

        /// <summary>
        /// Is true if any comments are public, false otherwise
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Update operations only
        /// Comment about the updates made.
        /// </summary>
        public Comment Comment { get; set; }

        /// <summary>
        /// Update operations only
        /// Requester of the updates made.
        /// </summary>
        public Requester Requester { get; set; }

        /// <summary>
        /// Sideloaded
        /// Comment Count for this Ticket
        /// </summary>
        public long CommentCount { get; set; }

        /// <summary>
        /// Sideloaded
        /// Incident Count for this ticket
        /// </summary>
        public long IncidentCount { get; set; }
    }
}
