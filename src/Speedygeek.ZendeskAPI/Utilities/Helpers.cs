﻿// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace Speedygeek.ZendeskAPI.Utilities
{
    /// <summary>
    /// basic helpers
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Merges the key/value pairs from d2 into d1, without overwriting those already set in d1.
        /// </summary>
        /// <typeparam name="TKey"> key Type </typeparam>
        /// <typeparam name="TValue"> value Type </typeparam>
        /// <param name="d1"> dictionary one</param>
        /// <param name="d2"> dictionary two</param>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2)
        {
            foreach (var kv in d2.Where(x => !d1.Keys.Contains(x.Key)))
            {
                d1.Add(kv);
            }
        }

        /// <summary>
        ///  Build Query String
        /// </summary>
        /// <param name="requestUri">base URI</param>
        /// <param name="queryStringParams">parameters to add</param>
        /// <returns>query string</returns>
        public static string BuildQueryString(this string requestUri, Dictionary<string, string> queryStringParams)
        {
            return QueryHelpers.AddQueryString(requestUri, queryStringParams);
        }

        /// <summary>
        /// converts in to string safely
        /// </summary>
        /// <param name="value">number to convert</param>
        /// <returns>string</returns>
        public static string ToInvariantString(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
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
            return value.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Converts to a comma separated list
        /// </summary>
        /// <param name="values"> list of </param>
        /// <returns>comma separated string</returns>
        public static string ToCsv(this IEnumerable<long> values)
        {
            return string.Join(",", values.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
        }

        /// <summary>
        /// Removes the whitespace
        /// </summary>
        /// <param name="original">the original string</param>
        /// <returns>string with no whitespace</returns>
        public static string RemoveWhitespace(this string original)
        {
            return Regex.Replace(original, @"\s+", string.Empty);
        }
    }
}
