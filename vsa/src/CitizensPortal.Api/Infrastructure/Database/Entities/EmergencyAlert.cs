using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents an emergency alert issued by the municipality
/// </summary>
public sealed class EmergencyAlert : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public string AlertType { get; set; } = string.Empty; // Fire, Flood, Storm, Earthquake, etc.
    public string Severity { get; set; } = string.Empty; // Info, Warning, Severe, Critical
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string? AffectedAreas { get; set; } // JSON array of affected locations
    public string? InstructionsJson { get; set; } // Emergency instructions as JSON

    public bool IsActive { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public Guid IssuedByUserId { get; set; }
    public string IssuedByUserName { get; set; } = string.Empty;

    public int AcknowledgementCount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Tracks citizen acknowledgment of emergency alerts
/// </summary>
public sealed class AlertAcknowledgement : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid AlertId { get; set; }
    public EmergencyAlert Alert { get; set; } = null!;

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public DateTime AcknowledgedAt { get; set; }
    public string? Location { get; set; }
    public bool IsSafe { get; set; }
}
