using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Ticket
{
    public class TicketDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string ResolutionSummary { get; set; }
        public List<TicketMessageDto> Messages { get; set; }
    }

    public class TicketMessageDto : EntityDto<Guid>
    {
        public string Message { get; set; }
        public bool IsFromCitizen { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
