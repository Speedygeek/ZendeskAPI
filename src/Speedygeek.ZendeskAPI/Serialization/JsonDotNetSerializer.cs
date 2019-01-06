// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Speedygeek.ZendeskAPI.Contract;

namespace Speedygeek.ZendeskAPI.Serialization
{
    /// <summary>
    /// ISerializer implementation that uses Newtonsoft Json.NET.
    /// </summary>
    public class JsonDotNetSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDotNetSerializer"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public JsonDotNetSerializer(JsonSerializerSettings settings)
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

#if DEBUG
            _serializerSettings.Formatting = Formatting.Indented;
#endif

            // needed so that we don't need to add attributes to every property in every model class.
            _serializerSettings.ContractResolver = ZendeskContractResolver.Instance;
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
                    return JsonSerializer.Create(_serializerSettings).Deserialize<T>(jr);
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
