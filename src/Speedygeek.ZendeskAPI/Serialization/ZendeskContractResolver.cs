// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Speedygeek.ZendeskAPI.Serialization
{
    /// <summary>
    /// Contract resolver that allows cleaner code
    /// </summary>
    public class ZendeskContractResolver : DefaultContractResolver
    {
        /// <summary>
        ///  Static Instance used to speed and memory
        /// </summary>
        public static readonly ZendeskContractResolver Instance = new ZendeskContractResolver();

        /// <summary>
        /// Initializes a new instance of the <see cref="ZendeskContractResolver"/> class.
        /// </summary>
        public ZendeskContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy(true, true);
        }

        /// <summary>
        /// Creates properties for the given Newtonsoft.Json.Serialization.JsonContract.
        /// </summary>
        /// <param name="member"> The type to create properties for.</param>
        /// <param name="memberSerialization"> The member serialization mode for the type.</param>
        /// <returns> Properties for the given Newtonsoft.Json.Serialization.JsonContract.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                property.DefaultValueHandling = DefaultValueHandling.Include;
            }

            if (!property.Writable)
            {
                var fi = GetBackingField(member.DeclaringType, property.UnderlyingName);
                if (fi != null)
                {
                    property.Writable = true;
                    property.ValueProvider = new ReflectionValueProvider(fi);
                }
            }

            return property;
        }

        private FieldInfo GetBackingField(Type type, string propertyName)
        {
            return type.GetTypeInfo().GetDeclaredField($"<{propertyName}>k__BackingField");
        }
    }
}
