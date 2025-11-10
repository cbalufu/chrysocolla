using System;
using Volo.Abp.Domain.Entities.Auditing;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Entities
{
    public class ApplicationStatusHistory : CreationAuditedEntity<Guid>
    {
        public Guid ApplicationId { get; set; }
        public Application Application { get; set; }

        public ApplicationStatus FromStatus { get; set; }
        public ApplicationStatus ToStatus { get; set; }
        public string Notes { get; set; }
        public Guid? ChangedByUserId { get; set; }

        protected ApplicationStatusHistory()
        {
        }

        public ApplicationStatusHistory(Guid id, Guid applicationId, ApplicationStatus fromStatus, ApplicationStatus toStatus, string notes = null)
        {
            Id = id;
            ApplicationId = applicationId;
            FromStatus = fromStatus;
            ToStatus = toStatus;
            Notes = notes;
        }
    }
}
