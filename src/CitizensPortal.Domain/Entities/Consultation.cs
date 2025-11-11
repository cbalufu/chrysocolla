using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a public consultation on policies/projects
    /// </summary>
    public class Consultation : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string DetailedInformation { get; set; }

        public ConsultationStatus Status { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Attachments like PDFs, presentations
        public string DocumentsJson { get; set; }

        public int ViewCount { get; set; }
        public int CommentCount { get; set; }

        public ICollection<ConsultationComment> Comments { get; set; }

        protected Consultation()
        {
            Comments = new List<ConsultationComment>();
        }

        public Consultation(Guid id, string title, DateTime startDate, DateTime endDate, Guid? tenantId = null) : base(id)
        {
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Status = ConsultationStatus.Upcoming;
            TenantId = tenantId;
            Comments = new List<ConsultationComment>();
        }
    }

    public class ConsultationComment : CreationAuditedEntity<Guid>
    {
        public Guid ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string Comment { get; set; }
        public bool IsPublic { get; set; }

        protected ConsultationComment() { }

        public ConsultationComment(Guid id, Guid consultationId, Guid citizenId, string comment, bool isPublic = true) : base(id)
        {
            ConsultationId = consultationId;
            CitizenId = citizenId;
            Comment = comment;
            IsPublic = isPublic;
        }
    }
}
