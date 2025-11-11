using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.IssueReport
{
    public class IssueReportDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IssueCategory Category { get; set; }
        public IssuePriority Priority { get; set; }
        public IssueStatus Status { get; set; }
        public string LocationAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string ResolutionNotes { get; set; }
        public List<IssueAttachmentDto> Attachments { get; set; }
        public List<IssueCommentDto> Comments { get; set; }
    }

    public class IssueAttachmentDto : EntityDto<Guid>
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class IssueCommentDto : EntityDto<Guid>
    {
        public string Comment { get; set; }
        public bool IsInternal { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
