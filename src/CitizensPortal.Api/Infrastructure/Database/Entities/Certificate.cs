using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a certificate issued by the municipality
/// </summary>
public sealed class Certificate : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string CertificateNumber { get; set; } = string.Empty; // Auto-generated unique number
    public string CertificateType { get; set; } = string.Empty; // BirthCertificate, ResidenceCertificate, etc.
    public string Title { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty; // Requested, Processing, Issued, Rejected
    public DateTime? IssuedAt { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public string? FileUrl { get; set; } // PDF file URL
    public string? VerificationCode { get; set; } // Public verification code

    public Guid? IssuedByUserId { get; set; }
    public string? IssuedByUserName { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
