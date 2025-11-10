using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CitizensPortal.Domain.Entities
{
    public class TicketMessage : CreationAuditedEntity<Guid>
    {
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string Message { get; set; }
        public bool IsFromCitizen { get; set; }
        public Guid? UserId { get; set; } // Staff user who sent the message

        protected TicketMessage()
        {
        }

        public TicketMessage(Guid id, Guid ticketId, string message, bool isFromCitizen)
        {
            Id = id;
            TicketId = ticketId;
            Message = message;
            IsFromCitizen = isFromCitizen;
        }
    }
}
