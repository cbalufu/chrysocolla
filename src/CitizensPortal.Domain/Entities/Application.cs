using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents an application (housing, licensing, permits, etc.) submitted by a citizen
    /// </summary>
    public class Application : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string ApplicationNumber { get; set; }
        public ApplicationType Type { get; set; }
        public ApplicationStatus Status { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        // Application specific data (JSON)
        public string ApplicationData { get; set; }

        // Tracking
        public DateTime SubmissionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? RejectionDate { get; set; }
        public Guid? ReviewedByUserId { get; set; }
        public string ReviewNotes { get; set; }

        public ICollection<ApplicationDocument> Documents { get; set; }
        public ICollection<ApplicationStatusHistory> StatusHistory { get; set; }

        protected Application()
        {
            Documents = new List<ApplicationDocument>();
            StatusHistory = new List<ApplicationStatusHistory>();
        }

        public Application(
            Guid id,
            Guid citizenId,
            ApplicationType type,
            string title,
            string description,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Type = type;
            Title = title;
            Description = description;
            Status = ApplicationStatus.Draft;
            SubmissionDate = DateTime.UtcNow;
            TenantId = tenantId;
            ApplicationNumber = GenerateApplicationNumber(type);
            Documents = new List<ApplicationDocument>();
            StatusHistory = new List<ApplicationStatusHistory>();
        }

        private string GenerateApplicationNumber(ApplicationType type)
        {
            var prefix = type switch
            {
                ApplicationType.Housing => "HSG",
                ApplicationType.BusinessLicense => "LIC",
                ApplicationType.BuildingPermit => "BLD",
                ApplicationType.ZoningPermit => "ZON",
                _ => "APP"
            };
            return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }
    }
}
