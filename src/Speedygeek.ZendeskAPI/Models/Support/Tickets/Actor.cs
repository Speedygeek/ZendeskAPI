// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Model of user that deleted a Ticket
    /// </summary>
    [DebuggerDisplay("Name: {Name}, Id: {Id}")]
    public class Actor
    {
        /// <summary>
        /// Automatically assigned when the entity is created
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
    }
}
