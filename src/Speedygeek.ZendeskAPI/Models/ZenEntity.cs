// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Speedygeek.ZendeskAPI.Models
{
    /// <summary>
    /// Base class for most models
    /// </summary>
    public abstract class ZenEntity
    {
        /// <summary>
        /// Automatically assigned when the entity is created
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// When this entity was created
        /// </summary>
        public DateTimeOffset CreatedAt { get; }

        /// <summary>
        /// When this entity was last updated
        /// </summary>
        public DateTimeOffset UpdatedAt { get; }

        /// <summary>
        /// The API URL of this entity
        /// </summary>
        public string URL { get; }
    }
}
