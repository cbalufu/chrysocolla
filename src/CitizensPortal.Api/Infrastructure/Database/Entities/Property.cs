using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a property registered in the municipality
/// </summary>
public sealed class Property : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid OwnerId { get; set; }
    public Citizen Owner { get; set; } = null!;

    public string PropertyNumber { get; set; } = string.Empty; // Unique property identifier
    public string PropertyType { get; set; } = string.Empty; // Residential, Commercial, Industrial, Agricultural
    public string Address { get; set; } = string.Empty;

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public decimal? LandArea { get; set; } // in square meters
    public decimal? BuildingArea { get; set; } // in square meters
    public int? NumberOfRooms { get; set; }
    public int? YearBuilt { get; set; }

    public decimal? AssessedValue { get; set; }
    public decimal? AnnualTaxAmount { get; set; }

    public string Status { get; set; } = string.Empty; // Active, Inactive, UnderConstruction
    public string? ZoningClassification { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
