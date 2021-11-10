// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.Serialization.Converters
{
    /// <summary>
    /// converts an array of mixed type to a single type
    /// </summary>
    internal class CollaboratorConverter_Old : JsonConverter
    {
        public override bool CanWrite { get => true; }

        public override bool CanConvert(Type objectType)
        {
            return typeof(List<Collaborator>).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);
            var collaborators = new List<Collaborator>();
            foreach (var item in token.Children())
            {
                if (item.Type == JTokenType.Object)
                {
                    var collaborator = item.ToObject<Collaborator>();
                    collaborators.Add(collaborator);
                }
                else if (item.Type == JTokenType.String)
                {
                    var collaborator = new Collaborator { Email = item.ToObject<string>() };
                    collaborators.Add(collaborator);
                }
                else if (item.Type == JTokenType.Integer)
                {
                    var collaborator = new Collaborator { Id = item.ToObject<long>() };
                    collaborators.Add(collaborator);
                }
            }

            return collaborators;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is List<Collaborator> collaborators && collaborators?.Count > 0)
            {
                writer.WriteStartArray();
                foreach (var collaborator in collaborators)
                {
                    if (collaborator.Id != 0)
                    {
                        serializer.Serialize(writer, collaborator.Id);
                    }
                    else if (!string.IsNullOrWhiteSpace(collaborator.Name) && !string.IsNullOrWhiteSpace(collaborator.Email))
                    {
                        serializer.Serialize(writer, collaborator);
                    }
                    else if (!string.IsNullOrWhiteSpace(collaborator.Email))
                    {
                        serializer.Serialize(writer, collaborator.Email);
                    }
                }

                writer.WriteEndArray();
            }
        }
    }
}
