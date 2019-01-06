// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Speedygeek.ZendeskAPI.Utilities
{
    /// <summary>
    /// basic helpers
    /// </summary>
    internal static class Helpers
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
    }
}
