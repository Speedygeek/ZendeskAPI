// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Speedygeek.ZendeskAPI.Utilities
{
    /// <summary>
    /// <see cref="Enum"/> Utilities
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Gets The Display Name for value of enum element as a lowered string
        /// </summary>
        /// <param name="value"> value to get as a safe string</param>
        /// <returns>string</returns>
        public static string GetDisplayName(this Enum value)
        {
            var att = value.GetType().GetMember(value.ToString())[0].GetCustomAttribute<DisplayAttribute>();
            if (att != null)
            {
                return att.Name.ToLowerInvariant();
            }
            else
            {
                return value.ToString().ToLowerInvariant();
            }
        }
    }
}
