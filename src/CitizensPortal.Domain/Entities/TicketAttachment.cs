using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CitizensPortal.Domain.Entities
{
    public class TicketAttachment : CreationAuditedEntity<Guid>
    {
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }

        protected TicketAttachment()
        {
        }

        public TicketAttachment(Guid id, Guid ticketId, string fileName, string fileUrl)
        {
            Id = id;
            TicketId = ticketId;
            FileName = fileName;
            FileUrl = fileUrl;
        }
    }
}
