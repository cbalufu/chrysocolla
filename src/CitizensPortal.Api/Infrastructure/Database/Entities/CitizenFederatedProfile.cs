namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Federated citizen profile that links a citizen's identity across multiple councils.
/// This entity is NOT tenant-scoped - it exists at the host level to enable cross-council federation.
/// Stores encrypted national identifier with hashed lookup index.
/// </summary>
public sealed class CitizenFederatedProfile
{
    public Guid Id { get; set; }

    /// <summary>
    /// Type of national identifier (National ID, TIN, Passport)
    /// </summary>
    public NationalIdType NationalIdType { get; set; }

    /// <summary>
    /// Encrypted national identifier (AES-256 encrypted)
    /// </summary>
    public string NationalIdEncrypted { get; set; } = string.Empty;

    /// <summary>
    /// SHA-256 hash of the national identifier for efficient lookups
    /// </summary>
    public string NationalIdHash { get; set; } = string.Empty;

    /// <summary>
    /// Current verification status of this identity
    /// </summary>
    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;

    /// <summary>
    /// Method used to verify this identity
    /// </summary>
    public VerificationMethod? VerificationMethod { get; set; }

    /// <summary>
    /// When the identity was verified
    /// </summary>
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// Which tenant/council verified this identity
    /// </summary>
    public Guid? VerifiedByTenantId { get; set; }

    /// <summary>
    /// Citizen's consent status for cross-council data sharing (POPIA compliance)
    /// </summary>
    public ConsentStatus ConsentStatus { get; set; } = ConsentStatus.NotProvided;

    /// <summary>
    /// When consent was granted or revoked
    /// </summary>
    public DateTime? ConsentChangedAt { get; set; }

    /// <summary>
    /// Profile creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Navigation property to linked citizen profiles across tenants
    /// </summary>
    public ICollection<LinkedCitizenProfile> LinkedProfiles { get; set; } = new List<LinkedCitizenProfile>();
}
