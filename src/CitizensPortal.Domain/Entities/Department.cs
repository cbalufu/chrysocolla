using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a municipal department
    /// </summary>
    public class Department : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Location { get; set; }

        public bool IsActive { get; set; }
        public bool AllowsAppointments { get; set; }

        public ICollection<AppointmentSlot> AppointmentSlots { get; set; }

        protected Department()
        {
            AppointmentSlots = new List<AppointmentSlot>();
        }

        public Department(Guid id, string name, string code, Guid? tenantId = null) : base(id)
        {
            Name = name;
            Code = code;
            IsActive = true;
            AllowsAppointments = true;
            TenantId = tenantId;
            AppointmentSlots = new List<AppointmentSlot>();
        }
    }
}
