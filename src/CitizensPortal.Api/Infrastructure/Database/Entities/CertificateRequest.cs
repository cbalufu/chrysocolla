using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a citizen's request for an official certificate
/// </summary>
public sealed class CertificateRequest : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string ReferenceNumber { get; set; } = string.Empty; // Auto-generated: CERT-{TYPE}-{DATE}-{GUID}
    public string CertificateType { get; set; } = string.Empty; // Birth, Death, Marriage, Residence, GoodConduct, TaxClearance
    public string Status { get; set; } = string.Empty; // Submitted, UnderReview, Approved, Ready, Collected, Rejected
    public string Purpose { get; set; } = string.Empty;
    public string DeliveryMethod { get; set; } = string.Empty; // Collection, Email, Postal

    public decimal ApplicationFee { get; set; }
    public bool IsPaid { get; set; } = false;

    public DateTime? ApprovalDate { get; set; }
    public DateTime? CollectionDate { get; set; }
    public string? RejectionReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
