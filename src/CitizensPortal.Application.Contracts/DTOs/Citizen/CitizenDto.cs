using System;
using Volo.Abp.Application.Dtos;

namespace CitizensPortal.Application.Contracts.DTOs.Citizen
{
    public class CitizenDto : FullAuditedEntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PropertyReference { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerificationDate { get; set; }
    }
}
