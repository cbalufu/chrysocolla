using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CitizensPortal.Domain.Entities
{
    public class ApplicationDocument : CreationAuditedEntity<Guid>
    {
        public Guid ApplicationId { get; set; }
        public Application Application { get; set; }

        public string DocumentName { get; set; }
        public string DocumentType { get; set; } // "ID", "ProofOfResidence", "Certificate", etc.
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public bool IsVerified { get; set; }

        protected ApplicationDocument()
        {
        }

        public ApplicationDocument(Guid id, Guid applicationId, string documentName, string documentType, string fileUrl)
        {
            Id = id;
            ApplicationId = applicationId;
            DocumentName = documentName;
            DocumentType = documentType;
            FileUrl = fileUrl;
            IsVerified = false;
        }
    }
}
