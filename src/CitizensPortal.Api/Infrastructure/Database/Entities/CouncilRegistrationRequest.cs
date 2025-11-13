namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a council/local authority registration request
/// This entity is NOT multi-tenant as it exists before tenant creation
/// </summary>
public sealed class CouncilRegistrationRequest
{
    public Guid Id { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    // Council Information
    public string CouncilName { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty; // Official government registration number
    public string Region { get; set; } = string.Empty; // Province/Region/State

    // Contact Information
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;

    // Admin User Information (will be created upon approval)
    public string AdminName { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;

    // Address Information
    public string PhysicalAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    // Status and Review
    public CouncilRegistrationStatus Status { get; set; } = CouncilRegistrationStatus.PendingApproval;

    public DateTime SubmittedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public string? ReviewerName { get; set; }

    public string? RejectionReason { get; set; }
    public DateTime? ExpiresAt { get; set; }

    // Created Tenant Reference (set after approval)
    public Guid? TenantId { get; set; }
    public Guid? AdminUserId { get; set; }
}
