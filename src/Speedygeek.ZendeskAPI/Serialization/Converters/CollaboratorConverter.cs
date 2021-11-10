// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.Serialization.Converters
{
    /// <summary>
    /// converts an array of mixed type to a single type
    /// </summary>
    public class CollaboratorConverter : JsonConverter<List<Collaborator>>
    {
        /// <inheritdoc/>
        public override List<Collaborator> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = new List<Collaborator>();
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return list;
                case JsonTokenType.StartArray:
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }
                        else if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            var item = JsonSerializer.Deserialize<Collaborator>(ref reader, options);
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }
                        else if (reader.TokenType == JsonTokenType.String)
                        {
                            var collaborator = new Collaborator { Email = reader.GetString() };
                            list.Add(collaborator);
                        }
                        else if (reader.TokenType == JsonTokenType.Number)
                        {
                            var collaborator = new Collaborator { Id = reader.GetInt64() };
                            list.Add(collaborator);
                        }
                        else if (reader.TokenType == JsonTokenType.Null)
                        {
                            continue;
                        }
                    }

                    break;
            }

            return list;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, List<Collaborator> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
