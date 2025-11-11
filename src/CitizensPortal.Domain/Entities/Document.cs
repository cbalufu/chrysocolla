using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a document in the citizen's digital vault
    /// </summary>
    public class Document : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DocumentCategory Category { get; set; }
        public DocumentStatus Status { get; set; }
        public DocumentAccessLevel AccessLevel { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string FileHash { get; set; } // For integrity verification

        public DateTime? ExpiryDate { get; set; }
        public bool RequiresRenewal { get; set; }

        // Digital signature support
        public bool IsSigned { get; set; }
        public string SignatureData { get; set; }
        public DateTime? SignedDate { get; set; }
        public Guid? SignedByUserId { get; set; }

        // Related entity (if document is linked to application, license, etc.)
        public string RelatedEntityType { get; set; }
        public Guid? RelatedEntityId { get; set; }

        // Tags for easy searching
        public string Tags { get; set; }

        public ICollection<DocumentAccess> AccessLog { get; set; }

        protected Document()
        {
            AccessLog = new List<DocumentAccess>();
        }

        public Document(
            Guid id,
            Guid citizenId,
            string title,
            DocumentCategory category,
            string fileName,
            string fileUrl,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Title = title;
            Category = category;
            FileName = fileName;
            FileUrl = fileUrl;
            Status = DocumentStatus.Active;
            AccessLevel = DocumentAccessLevel.Private;
            TenantId = tenantId;
            AccessLog = new List<DocumentAccess>();
        }

        public void MarkAsExpired()
        {
            Status = DocumentStatus.Expired;
        }

        public void Sign(Guid userId, string signatureData)
        {
            IsSigned = true;
            SignatureData = signatureData;
            SignedDate = DateTime.UtcNow;
            SignedByUserId = userId;
        }
    }

    /// <summary>
    /// Tracks who accessed a document and when
    /// </summary>
    public class DocumentAccess : CreationAuditedEntity<Guid>
    {
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        public Guid? AccessedByUserId { get; set; }
        public string AccessType { get; set; } // "View", "Download", "Share"
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }

        protected DocumentAccess() { }

        public DocumentAccess(Guid id, Guid documentId, string accessType)
        {
            Id = id;
            DocumentId = documentId;
            AccessType = accessType;
        }
    }
}
