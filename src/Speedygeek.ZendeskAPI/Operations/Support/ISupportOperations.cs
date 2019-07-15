namespace Speedygeek.ZendeskAPI.Operations.Support
{
    public interface ISupportOperations
    {
        /// <summary>
        /// Ticket Operations
        /// </summary>
        ITicketOperations Tickets { get; }

        /// <summary>
        /// Attachment Operations
        /// </summary>
        IAttachmentOperations Attachments { get; }
    }
}
