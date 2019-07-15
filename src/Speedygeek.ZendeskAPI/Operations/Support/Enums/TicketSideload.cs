using System;
using System.Collections.Generic;
using System.Text;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    [Flags]
    public enum TicketSideload
    {
        None = 1,
        Users = 2,
        Groups = 4,
        Organizations = 8,
        LastAudits = 16,
        MetricSets = 32,
        Dates = 64,
        SharingAgreements = 128,
        IncidentCounts = 256,
        TicketForms = 512,
        CommentCount = 1024,
        MetricEvents = 2048,
        SLAS = 4096
    }
}
