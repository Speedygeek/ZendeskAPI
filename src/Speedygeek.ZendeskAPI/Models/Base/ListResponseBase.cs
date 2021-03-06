﻿// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.Models.Base
{
    /// <summary>
    /// Base class for all Operations Responses that are page-able
    /// </summary>
    public class ListResponseBase
    {
        private int _page = 0;
        private int _perPage = 100;
        private bool _updatedValues = false;

        /// <summary>
        /// URL of the next page
        /// </summary>
        public Uri NextPage { get; } = null;

        /// <summary>
        /// URl of the Previous page
        /// </summary>
        public Uri PreviousPage { get; } = null;

        /// <summary>
        /// Count of Items
        /// </summary>
        public int Count { get; } = 0;

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(Count / (double)PerPage);

        /// <summary>
        /// Current page number
        /// </summary>
        public int Page
        {
            get
            {
                if (!_updatedValues)
                {
                    UpdateValues();
                }

                return _page;
            }
        }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PerPage
        {
            get
            {
                if (!_updatedValues)
                {
                    UpdateValues();
                }

                return _perPage;
            }
        }

        private void UpdateValues()
        {
            var url = NextPage ?? PreviousPage;

            var queryString = QueryHelpers.ParseQuery(url.Query);

            if (PreviousPage == null)
            {
                _page = 1;
            }
            else if (NextPage == null)
            {
                _page = TotalPages;
            }
            else if (queryString.ContainsKey(Constants.Page))
            {
                _page = int.Parse(queryString[Constants.Page], CultureInfo.InvariantCulture) - 1;
            }

            if (queryString.ContainsKey(Constants.PerPage))
            {
                _perPage = int.Parse(queryString[Constants.PerPage], CultureInfo.InvariantCulture);
            }

            _updatedValues = true;
        }
    }
}
