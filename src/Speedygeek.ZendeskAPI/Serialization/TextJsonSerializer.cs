// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Speedygeek.ZendeskAPI.Serialization.Context;
using Speedygeek.ZendeskAPI.Serialization.Converters;

namespace Speedygeek.ZendeskAPI.Serialization
{
    /// <summary>
    /// ISerializer implementation that uses System.Text.Json.
    /// </summary>
    public class TextJsonSerializer : ISerializer
    {
        private readonly JsonNamingPolicy _policy;
        private readonly JsonSerializerOptions _options;

        // private readonly ZenJsonContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextJsonSerializer"/> class.
        /// </summary>
        public TextJsonSerializer()
        {
            _policy = new SnakeCaseNamePolicy();
            _options = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = _policy,
                DictionaryKeyPolicy = _policy,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                IgnoreReadOnlyProperties = false,
                Converters =
                {
                    new EnumToStringConverter(_policy),
                    new CollaboratorConverter(),
                },
            };

           // _context = new ZenJsonContext(_options);
        }

        /// <inheritdoc/>
        public T Deserialize<T>(Stream stream)
        {
            if (stream.Position != 0)
            {
                stream.Position = 0;
            }

            return JsonSerializer.Deserialize<T>(stream, _options);
        }

        /// <inheritdoc/>
        public string Serialize(object data)
        {
           return JsonSerializer.Serialize(data, _options);
        }
    }
}
