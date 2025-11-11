using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Application
{
    public class ApplicationDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public string ApplicationNumber { get; set; }
        public ApplicationType Type { get; set; }
        public ApplicationStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ApplicationData { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? RejectionDate { get; set; }
        public string ReviewNotes { get; set; }
        public List<ApplicationDocumentDto> Documents { get; set; }
    }

    public class ApplicationDocumentDto : EntityDto<Guid>
    {
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string FileUrl { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
