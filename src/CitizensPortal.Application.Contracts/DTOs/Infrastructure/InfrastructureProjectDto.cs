using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Infrastructure
{
    public class InfrastructureProjectDto : FullAuditedEntityDto<Guid>
    {
        public string ProjectNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
        public ProjectStatus Status { get; set; }
        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string BoundaryGeoJson { get; set; }
        public decimal Budget { get; set; }
        public decimal SpentAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PlannedCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public int PercentageComplete { get; set; }
        public string ContractorName { get; set; }
        public List<ProjectUpdateDto> Updates { get; set; }
    }

    public class ProjectUpdateDto : EntityDto<Guid>
    {
        public string UpdateTitle { get; set; }
        public string UpdateDescription { get; set; }
        public int PercentageComplete { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class CreateUpdateInfrastructureProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal Budget { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PlannedCompletionDate { get; set; }
        public string ContractorName { get; set; }
        public string ContractorContact { get; set; }
    }
}
