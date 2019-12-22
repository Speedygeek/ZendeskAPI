// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Speedygeek.ZendeskAPI.Operations.Support;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.Operations.Base
{
    /// <summary>
    /// pagination parameters
    /// </summary>
    public abstract class PageParameters
    {
        /// <summary>
        /// Number of the page to load
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// number of items per-page
        /// Note: Most of Zendesk API max at 100 items per-page
        /// </summary>
        public int PerPage { get; set; } = 100;

        /// <summary>
        /// Sort Order
        /// Defaults to Ascending
        /// </summary>
        public SortOrder SortOrder { get; set; } = SortOrder.None;

        /// <summary>
        /// Build Query string Parameters
        /// </summary>
        /// <returns> a Dictionary of Parameters</returns>
        public virtual Dictionary<string, string> ToParameters()
        {
            var parameters = new Dictionary<string, string>
            {
                { Constants.Page, PageNumber > 0 ? PageNumber.ToInvariantString() : Constants.DefaultPage },
                { Constants.PerPage, PerPage > 0 ? PerPage.ToInvariantString() : Constants.DefaultPageSize },
            };

            if (SortOrder != SortOrder.None)
            {
                parameters.Add(Constants.SortOrder, SortOrder.GetDisplayName());
            }

            return parameters;
        }
    }
}
