using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a service request from a citizen
    /// </summary>
    public class ServiceRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string RequestNumber { get; set; }
        public ServiceRequestType Type { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public ServiceRequestPriority Priority { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        // Location details
        public string ServiceAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Preferred scheduling
        public DateTime? PreferredDate { get; set; }
        public string PreferredTimeSlot { get; set; }

        // Assignment
        public Guid? AssignedToUserId { get; set; }
        public Guid? AssignedDepartmentId { get; set; }

        // Scheduling
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        // Completion details
        public string CompletionNotes { get; set; }
        public string WorkPerformed { get; set; }

        // Attachments
        public ICollection<ServiceRequestAttachment> Attachments { get; set; }

        // Updates
        public ICollection<ServiceRequestUpdate> Updates { get; set; }

        // Citizen feedback
        public int? Rating { get; set; }
        public string Feedback { get; set; }

        protected ServiceRequest()
        {
            Attachments = new List<ServiceRequestAttachment>();
            Updates = new List<ServiceRequestUpdate>();
        }

        public ServiceRequest(
            Guid id,
            Guid citizenId,
            ServiceRequestType type,
            string title,
            string description,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Type = type;
            Title = title;
            Description = description;
            Status = ServiceRequestStatus.Submitted;
            Priority = ServiceRequestPriority.Medium;
            TenantId = tenantId;
            RequestNumber = GenerateRequestNumber();
            Attachments = new List<ServiceRequestAttachment>();
            Updates = new List<ServiceRequestUpdate>();
        }

        private string GenerateRequestNumber()
        {
            return $"SRV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public void Schedule(DateTime scheduledDate)
        {
            ScheduledDate = scheduledDate;
            Status = ServiceRequestStatus.Scheduled;
        }

        public void Complete(string workPerformed)
        {
            Status = ServiceRequestStatus.Completed;
            CompletedDate = DateTime.UtcNow;
            WorkPerformed = workPerformed;
        }
    }

    public class ServiceRequestAttachment : CreationAuditedEntity<Guid>
    {
        public Guid ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }

        protected ServiceRequestAttachment() { }
    }

    public class ServiceRequestUpdate : CreationAuditedEntity<Guid>
    {
        public Guid ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }

        public string UpdateMessage { get; set; }
        public ServiceRequestStatus NewStatus { get; set; }
        public Guid? UpdatedByUserId { get; set; }

        protected ServiceRequestUpdate() { }
    }
}
