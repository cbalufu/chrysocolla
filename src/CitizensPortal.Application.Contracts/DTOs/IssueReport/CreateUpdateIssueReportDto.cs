using System.ComponentModel.DataAnnotations;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.IssueReport
{
    public class CreateUpdateIssueReportDto
    {
        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public string Description { get; set; }

        [Required]
        public IssueCategory Category { get; set; }

        public IssuePriority Priority { get; set; } = IssuePriority.Medium;

        [StringLength(512)]
        public string LocationAddress { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
