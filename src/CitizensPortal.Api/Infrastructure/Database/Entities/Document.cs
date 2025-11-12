using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a document in the system (citizen documents, certificates, etc.)
/// </summary>
public sealed class Document : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string DocumentType { get; set; } = string.Empty; // BirthCertificate, ResidencePermit, etc.
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty; // Pending, Verified, Rejected, Expired
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public Guid? VerifiedByUserId { get; set; }

    public DateTime? ExpiryDate { get; set; }
    public string? VerificationCode { get; set; } // For public verification

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
