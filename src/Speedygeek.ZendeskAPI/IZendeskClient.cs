// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.CompilerServices;
using Speedygeek.ZendeskAPI.Operations.Support;

[assembly: InternalsVisibleTo("Speedygeek.ZendeskAPI.UnitTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100016fb807ccaaea498170a4d63538e5415851d13ecaf62aa7dfb8d898a2d83a5bc17ef1eb3dcab33e073d8f24a950830c94baeb0c5aa68cff782d2d7c4b60ee90568776a68443c1e80ae20a09628904bb04bf1c14d496199ed2c78a4d30aa2fbd3e4b7a864d60ab417815fc06fb2e4675d4eb44132840500de3b7cdb43767c4dc")]

namespace Speedygeek.ZendeskAPI
{
    /// <summary>
    /// Describes the basic setup of a <see cref="ZendeskClient"/>
    /// </summary>
    public interface IZendeskClient
    {
        /// <summary>
        /// Support API for Zendesk
        /// </summary>
        ISupportOperations Support { get; }
    }
}
