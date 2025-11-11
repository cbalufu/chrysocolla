using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a service request made by a citizen
/// </summary>
public sealed class ServiceRequest : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string RequestNumber { get; set; } = string.Empty; // Auto-generated
    public string ServiceType { get; set; } = string.Empty; // Waste Collection, Street Repair, etc.
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty; // Submitted, Assigned, InProgress, Completed, Cancelled
    public string Priority { get; set; } = string.Empty; // Low, Medium, High

    public string? Location { get; set; }
    public DateTime? PreferredServiceDate { get; set; }

    public Guid? AssignedToUserId { get; set; }
    public DateTime? AssignedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
