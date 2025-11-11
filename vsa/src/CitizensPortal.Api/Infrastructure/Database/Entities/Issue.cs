using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents an issue or complaint reported by a citizen
/// </summary>
public sealed class Issue : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Infrastructure, Safety, Waste Management, etc.
    public string Status { get; set; } = string.Empty; // Reported, InProgress, Resolved, Closed
    public string Priority { get; set; } = string.Empty; // Low, Medium, High, Critical

    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public string? ImageUrls { get; set; } // JSON array of image URLs

    public Guid? AssignedToUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public ICollection<IssueComment> Comments { get; set; } = new List<IssueComment>();
}

/// <summary>
/// Comments on an issue
/// </summary>
public sealed class IssueComment : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid IssueId { get; set; }
    public Issue Issue { get; set; } = null!;

    public Guid UserId { get; set; } // Can be citizen or staff
    public string UserName { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public bool IsInternal { get; set; } // Internal staff notes vs public comments

    public DateTime CreatedAt { get; set; }
}
