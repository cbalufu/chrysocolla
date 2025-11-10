using System.ComponentModel.DataAnnotations;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Application
{
    public class CreateUpdateApplicationDto
    {
        [Required]
        public ApplicationType Type { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public string Description { get; set; }

        public string ApplicationData { get; set; }
    }
}
