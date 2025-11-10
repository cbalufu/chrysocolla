using System.ComponentModel.DataAnnotations;

namespace CitizensPortal.Application.Contracts.DTOs.Citizen
{
    public class CreateUpdateCitizenDto
    {
        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string NationalId { get; set; }

        [StringLength(512)]
        public string Address { get; set; }

        [StringLength(128)]
        public string City { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string PropertyReference { get; set; }
    }
}
