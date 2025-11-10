using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CitizensPortal.Domain.Entities
{
    public class IssueAttachment : CreationAuditedEntity<Guid>
    {
        public Guid IssueReportId { get; set; }
        public IssueReport IssueReport { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }

        protected IssueAttachment()
        {
        }

        public IssueAttachment(Guid id, Guid issueReportId, string fileName, string fileUrl, string contentType, long fileSize)
        {
            Id = id;
            IssueReportId = issueReportId;
            FileName = fileName;
            FileUrl = fileUrl;
            ContentType = contentType;
            FileSize = fileSize;
        }
    }
}
