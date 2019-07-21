// Copyright (c) Elizabeth Schneider. All Rights Reserved.
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

#pragma warning disable CA1721 // Property names should not match get methods
        /// <summary>
        /// URL of the next page
        /// </summary>
        public Uri NextPage { get; } = null;
#pragma warning restore CA1721 // Property names should not match get methods

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

            if (PreviousPage == null)
            {
                _page = 1;
            }

            if (NextPage == null)
            {
                _page = TotalPages;
            }

            var queryString = QueryHelpers.ParseQuery(url.Query);
            _page = int.Parse(queryString[Constants.Page], CultureInfo.InvariantCulture);
            _perPage = int.Parse(queryString[Constants.PerPage], CultureInfo.InvariantCulture);

            _updatedValues = true;
        }
    }
}
