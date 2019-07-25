// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;
using Speedygeek.ZendeskAPI.Serialization.Converters;

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// custom Field value for Ticket
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// Id of the custom field for which the value should be set.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// values to set to field
        /// </summary>
        [JsonConverter(typeof(SingleOrListConverter<string>))]
        public IList<string> Value { get; set; } = new List<string>();
    }
}
