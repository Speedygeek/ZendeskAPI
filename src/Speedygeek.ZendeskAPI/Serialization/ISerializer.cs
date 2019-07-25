// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;

namespace Speedygeek.ZendeskAPI.Serialization
{
    /// <summary>
    /// Contract for serializing and deserializing objects.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes an object to a string representation.
        /// </summary>
        /// <param name="data">object to be serialized</param>
        /// <returns> a json <see cref="string"/> representation of the original object</returns>
        string Serialize(object data);

        /// <summary>
        /// Deserializes an object from a stream representation.
        /// </summary>
        /// <typeparam name="T">the type of the object to deserializes as</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns> HTTP content used for request</returns>
        T Deserialize<T>(Stream stream);
    }
}
