// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Options that can be side-loaded with Tickets
    /// </summary>
    [Flags]
    public enum TicketSideloads
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Users
        /// </summary>
        Users = 2,

        /// <summary>
        /// Groups
        /// </summary>
        Groups = 4,

        /// <summary>
        /// Organizations
        /// </summary>
        Organizations = 8,

        /// <summary>
        /// Last Audits
        /// </summary>
        LastAudits = 16,

        /// <summary>
        /// Metric Sets
        /// </summary>
        MetricSets = 32,

        /// <summary>
        /// Dates
        /// </summary>
        Dates = 64,

        /// <summary>
        /// Sharing Agreements
        /// </summary>
        SharingAgreements = 128,

        /// <summary>
        /// Incidents Counts
        /// </summary>
        IncidentCounts = 256,

        /// <summary>
        /// Ticket Forms
        /// </summary>
        TicketForms = 512,

        /// <summary>
        /// Comment Count
        /// </summary>
        CommentCount = 1024,

        /// <summary>
        /// Metric Events
        /// </summary>
        MetricEvents = 2048,

        /// <summary>
        /// SLAS
        /// </summary>
        SLAS = 4096,
    }
}
