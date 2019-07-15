using System;
using System.Collections.Generic;
using System.Text;

namespace Speedygeek.ZendeskAPI.Operations.Support
{
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


    }
}
