// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.Serialization.Context
{
    internal class SnakeCaseNamePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToSnakeCase();
    }
}
