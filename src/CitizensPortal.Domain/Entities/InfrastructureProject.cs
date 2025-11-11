using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents an infrastructure project in the municipality
    /// </summary>
    public class InfrastructureProject : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string ProjectNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
        public ProjectStatus Status { get; set; }

        public string Location { get; set; }
        public string Ward { get; set; } // Ward/district where project is located
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Boundary polygon for area projects (stored as JSON GeoJSON)
        public string BoundaryGeoJson { get; set; }

        public decimal Budget { get; set; }
        public decimal SpentAmount { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? PlannedCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }

        public int PercentageComplete { get; set; }

        // Contractor information
        public string ContractorName { get; set; }
        public string ContractorContact { get; set; }

        // Project manager
        public Guid? ProjectManagerId { get; set; }

        public ICollection<ProjectUpdate> Updates { get; set; }
        public ICollection<ProjectImage> Images { get; set; }

        protected InfrastructureProject()
        {
            Updates = new List<ProjectUpdate>();
            Images = new List<ProjectImage>();
        }

        public InfrastructureProject(
            Guid id,
            string name,
            ProjectCategory category,
            decimal budget,
            Guid? tenantId = null) : base(id)
        {
            Name = name;
            Category = category;
            Budget = budget;
            Status = ProjectStatus.Planned;
            PercentageComplete = 0;
            SpentAmount = 0;
            TenantId = tenantId;
            ProjectNumber = GenerateProjectNumber();
            Updates = new List<ProjectUpdate>();
            Images = new List<ProjectImage>();
        }

        private string GenerateProjectNumber()
        {
            return $"PROJ-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }
    }

    /// <summary>
    /// Progress updates for infrastructure projects
    /// </summary>
    public class ProjectUpdate : CreationAuditedEntity<Guid>
    {
        public Guid InfrastructureProjectId { get; set; }
        public InfrastructureProject InfrastructureProject { get; set; }

        public string UpdateTitle { get; set; }
        public string UpdateDescription { get; set; }
        public int PercentageComplete { get; set; }
        public DateTime UpdateDate { get; set; }

        protected ProjectUpdate() { }

        public ProjectUpdate(Guid id, Guid projectId, string title, string description, int percentageComplete)
        {
            Id = id;
            InfrastructureProjectId = projectId;
            UpdateTitle = title;
            UpdateDescription = description;
            PercentageComplete = percentageComplete;
            UpdateDate = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Images of infrastructure projects
    /// </summary>
    public class ProjectImage : CreationAuditedEntity<Guid>
    {
        public Guid InfrastructureProjectId { get; set; }
        public InfrastructureProject InfrastructureProject { get; set; }

        public string FileName { get; set; }
        public string ImageUrl { get; set; }
        public string Caption { get; set; }
        public DateTime TakenDate { get; set; }

        protected ProjectImage() { }

        public ProjectImage(Guid id, Guid projectId, string fileName, string imageUrl)
        {
            Id = id;
            InfrastructureProjectId = projectId;
            FileName = fileName;
            ImageUrl = imageUrl;
            TakenDate = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Represents a service coverage area
    /// </summary>
    public class ServiceArea : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public ServiceAreaType Type { get; set; }

        // GeoJSON polygon defining the area
        public string AreaGeoJson { get; set; }

        public bool IsActive { get; set; }

        protected ServiceArea() { }

        public ServiceArea(Guid id, string name, ServiceAreaType type, Guid? tenantId = null) : base(id)
        {
            Name = name;
            Type = type;
            IsActive = true;
            TenantId = tenantId;
        }
    }
}
