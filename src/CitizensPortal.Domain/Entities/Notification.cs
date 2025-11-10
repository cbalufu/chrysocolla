using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a notification sent to a citizen
    /// </summary>
    public class Notification : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public NotificationPriority Priority { get; set; }

        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }

        // Link to related entity
        public string RelatedEntityType { get; set; } // "Bill", "Application", "IssueReport", etc.
        public Guid? RelatedEntityId { get; set; }

        protected Notification()
        {
        }

        public Notification(
            Guid id,
            Guid citizenId,
            string title,
            string message,
            NotificationType type,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Title = title;
            Message = message;
            Type = type;
            Priority = NotificationPriority.Normal;
            IsRead = false;
            TenantId = tenantId;
        }

        public void MarkAsRead()
        {
            IsRead = true;
            ReadDate = DateTime.UtcNow;
        }
    }
}
