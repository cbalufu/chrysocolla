using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CitizensPortal.Domain.Entities
{
    public class IssueComment : CreationAuditedEntity<Guid>
    {
        public Guid IssueReportId { get; set; }
        public IssueReport IssueReport { get; set; }

        public string Comment { get; set; }
        public bool IsInternal { get; set; } // True if comment is from staff/admin
        public Guid? UserId { get; set; } // Staff user who made the comment

        protected IssueComment()
        {
        }

        public IssueComment(Guid id, Guid issueReportId, string comment, bool isInternal = false)
        {
            Id = id;
            IssueReportId = issueReportId;
            Comment = comment;
            IsInternal = isInternal;
        }
    }
}
