using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents an issue reported by a citizen
    /// </summary>
    public class IssueReport : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public IssueCategory Category { get; set; }
        public IssuePriority Priority { get; set; }
        public IssueStatus Status { get; set; }

        // Location information
        public string LocationAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Tracking information
        public string ReferenceNumber { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string ResolutionNotes { get; set; }

        public ICollection<IssueAttachment> Attachments { get; set; }
        public ICollection<IssueComment> Comments { get; set; }

        protected IssueReport()
        {
            Attachments = new List<IssueAttachment>();
            Comments = new List<IssueComment>();
        }

        public IssueReport(
            Guid id,
            Guid citizenId,
            string title,
            string description,
            IssueCategory category,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Title = title;
            Description = description;
            Category = category;
            Status = IssueStatus.New;
            Priority = IssuePriority.Medium;
            TenantId = tenantId;
            ReferenceNumber = GenerateReferenceNumber();
            Attachments = new List<IssueAttachment>();
            Comments = new List<IssueComment>();
        }

        private string GenerateReferenceNumber()
        {
            return $"ISS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
