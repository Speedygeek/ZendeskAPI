﻿// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Speedygeek.ZendeskAPI.Serialization.Converters;

namespace Speedygeek.ZendeskAPI.Serialization
{
    /// <summary>
    /// ISerializer implementation that uses Newtonsoft Json.NET.
    /// </summary>
    public class JsonDotNetSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JsonSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDotNetSerializer"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public JsonDotNetSerializer(JsonSerializerSettings settings = null)
        {
            _serializerSettings = settings ?? new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
            _serializerSettings.Converters.Add(new CollaboratorConverter());

#if DEBUG
            _serializerSettings.Formatting = Formatting.Indented;
#endif

            // needed so that we don't need to add attributes to every property in every model class.
            _serializerSettings.ContractResolver = ZendeskContractResolver.Instance;

            _serializer = JsonSerializer.CreateDefault(_serializerSettings);
        }

        /// <inheritdoc/>
        public T Deserialize<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s, _serializerSettings);
        }

        /// <inheritdoc/>
        public T Deserialize<T>(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    return _serializer.Deserialize<T>(jr);
                }
            }
        }

        /// <inheritdoc/>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _serializerSettings);
        }
    }
}
