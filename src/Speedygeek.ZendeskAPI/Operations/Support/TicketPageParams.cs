// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Operations.Base;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Pagination parameters for <see cref="TicketOperations"/>
    /// </summary>
    public class TicketPageParams : PageParameters
    {
        /// <summary>
        /// Sort By
        /// </summary>
        public TicketSortBy SortBy { get; set; } = TicketSortBy.None;

        /// <summary>
        /// Builds the query string parameters
        /// </summary>
        /// <returns>
        /// <see cref="Dictionary{TKey, TValue}"/>
        /// </returns>
        public override Dictionary<string, string> ToParameters()
        {
            var paramerters = base.ToParameters();

            if (SortBy != TicketSortBy.None)
            {
                paramerters.Add(Constants.SortBy, SortBy.GetDisplayName());
            }

            return paramerters;
        }
    }
}
