namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Links a federated profile to a tenant-specific citizen record.
/// This entity is NOT tenant-scoped - it exists at the host level.
/// Creates the "spoke" connections from the "hub" CitizenFederatedProfile.
/// </summary>
public sealed class LinkedCitizenProfile
{
    public Guid Id { get; set; }

    /// <summary>
    /// Reference to the federated profile (hub)
    /// </summary>
    public Guid FederatedProfileId { get; set; }

    /// <summary>
    /// Navigation property to federated profile
    /// </summary>
    public CitizenFederatedProfile FederatedProfile { get; set; } = null!;

    /// <summary>
    /// Tenant/Council ID where the citizen is registered
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Citizen ID within the tenant
    /// </summary>
    public Guid CitizenId { get; set; }

    /// <summary>
    /// When this link was created
    /// </summary>
    public DateTime LinkedAt { get; set; }

    /// <summary>
    /// Optional: when this link was removed (for audit trail)
    /// </summary>
    public DateTime? UnlinkedAt { get; set; }

    /// <summary>
    /// Whether this link is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;
}
