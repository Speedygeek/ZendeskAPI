// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Speedygeek.ZendeskAPI.Serialization.Converters
{
    /// <summary>
    /// Convert for Enum to String
    /// </summary>
    public class EnumToStringConverter : JsonConverterFactory
    {
        private readonly JsonNamingPolicy _namingPolicy;
        private readonly bool _allowIntegerValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumToStringConverter"/> class.
        /// </summary>
        public EnumToStringConverter()
            : this(namingPolicy: null, allowIntegerValues: true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumToStringConverter"/> class.
        /// </summary>
        /// <param name="namingPolicy">name policy used in name casing </param>
        /// <param name="allowIntegerValues"> can numbers be used</param>
        public EnumToStringConverter(JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true)
        {
            _namingPolicy = namingPolicy;
            _allowIntegerValues = allowIntegerValues;
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        /// <inheritdoc/>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(
                typeof(Converter<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { _namingPolicy, _allowIntegerValues },
                culture: null);
        }

        private class Converter<T> : JsonConverter<T>
            where T : struct, Enum
        {
            private static readonly Type _enumType = typeof(T);
            private static readonly TypeCode _enumTypeCode = Type.GetTypeCode(_enumType);
            private readonly bool _allowIntegerValues;
            private readonly bool _isFlags;
            private readonly Dictionary<ulong, EnumInfo> _rawToTransformed;
            private readonly Dictionary<string, EnumInfo> _transformedToRaw;

            public Converter(JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true)
            {
                _allowIntegerValues = allowIntegerValues;

                _isFlags = _enumType.IsDefined(typeof(FlagsAttribute), true);

                var builtInNames = _enumType.GetEnumNames();
                var builtInValues = _enumType.GetEnumValues();

                _rawToTransformed = new Dictionary<ulong, EnumInfo>();
                _transformedToRaw = new Dictionary<string, EnumInfo>();

                for (var i = 0; i < builtInNames.Length; i++)
                {
                    var enumValue = (T)builtInValues.GetValue(i);
                    var rawValue = GetEnumValue(enumValue);

                    var name = builtInNames[i];

                    string transformedName;
                    if (namingPolicy == null)
                    {
                        var field = _enumType.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!;
                        var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>(true);
                        transformedName = enumMemberAttribute?.Value ?? name;
                    }
                    else
                    {
                        transformedName = namingPolicy.ConvertName(name) ?? name;
                    }

                    _rawToTransformed[rawValue] = new EnumInfo
                    {
                        Name = transformedName,
                        EnumValue = enumValue,
                        RawValue = rawValue,
                    };
                    _transformedToRaw[transformedName] = new EnumInfo
                    {
                        Name = name,
                        EnumValue = enumValue,
                        RawValue = rawValue,
                    };
                }
            }

            private static ulong GetEnumValue(object value)
            {
                switch (_enumTypeCode)
                {
                    case TypeCode.Int32:
                        return (ulong)(int)value;
                    case TypeCode.UInt32:
                        return (uint)value;
                    case TypeCode.UInt64:
                        return (ulong)value;
                    case TypeCode.Int64:
                        return (ulong)(long)value;
                    case TypeCode.SByte:
                        return (ulong)(sbyte)value;
                    case TypeCode.Byte:
                        return (byte)value;
                    case TypeCode.Int16:
                        return (ulong)(short)value;
                    case TypeCode.UInt16:
                        return (ushort)value;
                }

                throw new NotSupportedException();
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var token = reader.TokenType;

                if (token == JsonTokenType.String)
                {
                    var enumString = reader.GetString();

                    // Case sensitive search attempted first.
                    if (_transformedToRaw.TryGetValue(enumString, out var enumInfo))
                    {
                        return (T)Enum.ToObject(_enumType, enumInfo.RawValue);
                    }

                    if (_isFlags)
                    {
                        ulong calculatedValue = 0;

                        var flagValues = enumString.Split(", ");
                        foreach (var flagValue in flagValues)
                        {
                            // Case sensitive search attempted first.
                            if (_transformedToRaw.TryGetValue(flagValue, out enumInfo))
                            {
                                calculatedValue |= enumInfo.RawValue;
                            }
                            else
                            {
                                // Case insensitive search attempted second.
                                var matched = false;
                                foreach (var enumItem in _transformedToRaw)
                                {
                                    if (string.Equals(enumItem.Key, flagValue, StringComparison.OrdinalIgnoreCase))
                                    {
                                        calculatedValue |= enumItem.Value.RawValue;
                                        matched = true;
                                        break;
                                    }
                                }

                                if (!matched)
                                {
                                    throw new NotSupportedException();
                                }
                            }
                        }

                        return (T)Enum.ToObject(_enumType, calculatedValue);
                    }
                    else
                    {
                        // Case insensitive search attempted second.
                        foreach (var enumItem in _transformedToRaw)
                        {
                            if (string.Equals(enumItem.Key, enumString, StringComparison.OrdinalIgnoreCase))
                            {
                                return (T)Enum.ToObject(_enumType, enumItem.Value.RawValue);
                            }
                        }
                    }

                    throw new NotSupportedException();
                }

                if (token != JsonTokenType.Number || !_allowIntegerValues)
                {
                    throw new NotSupportedException();
                }

                switch (_enumTypeCode)
                {
                    // Switch cases ordered by expected frequency
                    case TypeCode.Int32:
                        if (reader.TryGetInt32(out var int32))
                        {
                            return (T)Enum.ToObject(_enumType, int32);
                        }

                        break;
                    case TypeCode.UInt32:
                        if (reader.TryGetUInt32(out var uint32))
                        {
                            return (T)Enum.ToObject(_enumType, uint32);
                        }

                        break;
                    case TypeCode.UInt64:
                        if (reader.TryGetUInt64(out var uint64))
                        {
                            return (T)Enum.ToObject(_enumType, uint64);
                        }

                        break;
                    case TypeCode.Int64:
                        if (reader.TryGetInt64(out var int64))
                        {
                            return (T)Enum.ToObject(_enumType, int64);
                        }

                        break;
                    case TypeCode.SByte:
                        if (reader.TryGetSByte(out var byte8))
                        {
                            return (T)Enum.ToObject(_enumType, byte8);
                        }

                        break;
                    case TypeCode.Byte:
                        if (reader.TryGetByte(out var ubyte8))
                        {
                            return (T)Enum.ToObject(_enumType, ubyte8);
                        }

                        break;
                    case TypeCode.Int16:
                        if (reader.TryGetInt16(out var int16))
                        {
                            return (T)Enum.ToObject(_enumType, int16);
                        }

                        break;
                    case TypeCode.UInt16:
                        if (reader.TryGetUInt16(out var uint16))
                        {
                            return (T)Enum.ToObject(_enumType, uint16);
                        }

                        break;
                }

                throw new NotSupportedException();
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                var rawValue = GetEnumValue(value);

                if (_rawToTransformed.TryGetValue(rawValue, out var enumInfo))
                {
                    writer.WriteStringValue(enumInfo.Name);
                    return;
                }

                if (_isFlags)
                {
                    ulong calculatedValue = 0;

                    var builder = new StringBuilder();
                    foreach (var enumItem in _rawToTransformed)
                    {
                        enumInfo = enumItem.Value;

                        // Definitions with 'None' should hit the cache case.
                        if (!value.HasFlag(enumInfo.EnumValue)
                            || enumInfo.RawValue == 0)
                        {
                            continue;
                        }

                        // Track the value to make sure all bits are represented.
                        calculatedValue |= enumInfo.RawValue;

                        if (builder.Length > 0)
                        {
                            builder.Append(", ");
                        }

                        builder.Append(enumInfo.Name);
                    }

                    if (calculatedValue == rawValue)
                    {
                        writer.WriteStringValue(builder.ToString());
                        return;
                    }
                }

                if (!_allowIntegerValues)
                {
                    throw new NotSupportedException();
                }

                switch (_enumTypeCode)
                {
                    case TypeCode.Int32:
                        writer.WriteNumberValue((int)rawValue);
                        break;
                    case TypeCode.UInt32:
                        writer.WriteNumberValue((uint)rawValue);
                        break;
                    case TypeCode.UInt64:
                        writer.WriteNumberValue(rawValue);
                        break;
                    case TypeCode.Int64:
                        writer.WriteNumberValue((long)rawValue);
                        break;
                    case TypeCode.Int16:
                        writer.WriteNumberValue((short)rawValue);
                        break;
                    case TypeCode.UInt16:
                        writer.WriteNumberValue((ushort)rawValue);
                        break;
                    case TypeCode.Byte:
                        writer.WriteNumberValue((byte)rawValue);
                        break;
                    case TypeCode.SByte:
                        writer.WriteNumberValue((sbyte)rawValue);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            private class EnumInfo
            {
                /// <summary>
                /// Name
                /// </summary>
#pragma warning disable SA1401 // Fields should be private
                public string Name;
#pragma warning restore SA1401 // Fields should be private

                /// <summary>
                /// Enum value
                /// </summary>
#pragma warning disable SA1401 // Fields should be private
                public T EnumValue;
#pragma warning restore SA1401 // Fields should be private

                /// <summary>
                /// value
                /// </summary>
#pragma warning disable SA1401 // Fields should be private
                public ulong RawValue;
#pragma warning restore SA1401 // Fields should be private
            }
        }
    }
}
