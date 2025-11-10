using System;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Document
{
    public class DocumentDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DocumentCategory Category { get; set; }
        public DocumentStatus Status { get; set; }
        public DocumentAccessLevel AccessLevel { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool RequiresRenewal { get; set; }
        public bool IsSigned { get; set; }
        public DateTime? SignedDate { get; set; }
        public string RelatedEntityType { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string Tags { get; set; }
    }

    public class CreateUpdateDocumentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DocumentCategory Category { get; set; }
        public DocumentAccessLevel AccessLevel { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool RequiresRenewal { get; set; }
        public string RelatedEntityType { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string Tags { get; set; }
    }
}
