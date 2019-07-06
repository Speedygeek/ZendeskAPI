// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Speedygeek.ZendeskAPI.Models.Support;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Operations that can be done with <see cref="Ticket"/>
    /// </summary>
    public interface ITicketOperations
    {
        /// <summary>
        /// A single <see cref="Ticket"/>
        /// </summary>
        /// <param name="id">Id of the <see cref="Ticket"/> to load</param>
        /// <returns> Returns a number of <see cref="Ticket"/> properties though not the ticket comments.</returns>
        Task<TicketResponse> GetTicket(long id);

        /// <summary>
        ///  Create a new Ticket
        /// </summary>
        /// <param name="ticket"> <see cref="Ticket"/> to save</param>
        /// <returns>created <see cref="Ticket"/></returns>
        Task<TicketResponse> Create(Ticket ticket);
    }
}
