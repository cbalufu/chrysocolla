using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a support ticket created by a citizen
    /// </summary>
    public class Ticket : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }

        public Guid? AssignedToUserId { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string ResolutionSummary { get; set; }

        public ICollection<TicketMessage> Messages { get; set; }
        public ICollection<TicketAttachment> Attachments { get; set; }

        protected Ticket()
        {
            Messages = new List<TicketMessage>();
            Attachments = new List<TicketAttachment>();
        }

        public Ticket(
            Guid id,
            Guid citizenId,
            string subject,
            string description,
            TicketCategory category,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Subject = subject;
            Description = description;
            Category = category;
            Status = TicketStatus.Open;
            Priority = TicketPriority.Normal;
            TenantId = tenantId;
            TicketNumber = GenerateTicketNumber();
            Messages = new List<TicketMessage>();
            Attachments = new List<TicketAttachment>();
        }

        private string GenerateTicketNumber()
        {
            return $"TKT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
