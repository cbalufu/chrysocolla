using System.ComponentModel.DataAnnotations;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Ticket
{
    public class CreateUpdateTicketDto
    {
        [Required]
        [StringLength(256)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2048)]
        public string Description { get; set; }

        [Required]
        public TicketCategory Category { get; set; }

        public TicketPriority Priority { get; set; } = TicketPriority.Normal;
    }
}
