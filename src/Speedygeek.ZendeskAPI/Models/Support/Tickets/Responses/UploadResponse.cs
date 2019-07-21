﻿// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Speedygeek.ZendeskAPI.Models.Support.Tickets.Responses
{
    /// <summary>
    /// Upload Response
    /// </summary>
    public class UploadResponse
    {
        /// <summary>
        /// the items uploaded
        /// </summary>
        public Upload Upload { get; set; }
    }
}