// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Speedygeek.ZendeskAPI.Utilities
{
    /// <summary>
    /// <see cref="Enum"/> Utilities
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Gets The Enum Member Value for value of enum element as a lowered string
        /// </summary>
        /// <param name="value"> value to get as a safe string</param>
        /// <returns>string</returns>
        public static string GetEnumMemberValue(this Enum value)
        {
            var att = value.GetType().GetMember(value.ToString())[0].GetCustomAttribute<EnumMemberAttribute>();
            if (att != null)
            {
                return att.Value.ToLowerInvariant();
            }
            else
            {
                return value.ToString().ToSnakeCase().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Converts to a lowered string
        /// </summary>
        /// <typeparam name="TEnum">type of enum to convert </typeparam>
        /// <param name="value">enum option</param>
        /// <returns>value as string</returns>
        public static string ToLowerInvariantString<TEnum>(this TEnum value)
            where TEnum : Enum
        {
            var res = Enum.GetValues(value.GetType())
            .Cast<TEnum>()
            .Where(x => value.HasFlag(x) && (int)(object)x != 0)
            .Select(x => x.GetEnumMemberValue());

            return string.Join(",", res);
        }

        /// <summary>
        /// Is one flag passed
        /// </summary>
        /// <typeparam name="TEnum">type of enum to convert </typeparam>
        /// <param name="value">enum option</param>
        /// <returns>If only one flag is passed</returns>
        public static bool IsSingleFlag<TEnum>(this TEnum value)
            where TEnum : Enum
        {
            // see https://stackoverflow.com/questions/19376748/test-that-only-a-single-bit-is-set-in-flags-enum
            var bit = (int)(object)value;
            return bit != 0 && (bit & (bit - 1)) == 0;
        }
    }
}
