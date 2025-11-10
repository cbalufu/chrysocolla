using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a citizen/ratepayer in the system
    /// </summary>
    public class Citizen : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        // Ratepayer specific information
        public string PropertyReference { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerificationDate { get; set; }

        protected Citizen()
        {
        }

        public Citizen(
            Guid id,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string nationalId,
            Guid? tenantId = null) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            NationalId = nationalId;
            TenantId = tenantId;
            IsVerified = false;
        }
    }
}
