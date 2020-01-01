// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
    /// <summary>
    /// Support Operations
    /// </summary>
    public class SupportOperations : ISupportOperations
    {
        private readonly IRESTClient _restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportOperations"/> class.
        /// </summary>
        /// <param name="restClient">client used to make HTTP calls with</param>
        public SupportOperations(IRESTClient restClient)
        {
            _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
        }

        private Lazy<ITicketOperations> TicketsLazy => new Lazy<ITicketOperations>(() => new TicketOperations(_restClient));

        /// <inheritdoc />
        public ITicketOperations Tickets => TicketsLazy.Value;

        private Lazy<IAttachmentOperations> AttachmentLazy => new Lazy<IAttachmentOperations>(() => new AttachmentOperations(_restClient));

        /// <inheritdoc />
        public IAttachmentOperations Attachments => AttachmentLazy.Value;

        private Lazy<IUserOperations> UserLazy => new Lazy<IUserOperations>(() => new UserOperations(_restClient));

        /// <inheritdoc />
        public IUserOperations Users => UserLazy.Value;
    }
}
