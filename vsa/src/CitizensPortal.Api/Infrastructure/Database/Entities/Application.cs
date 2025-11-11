using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a citizen application for permits, licenses, or services
/// </summary>
public sealed class Application : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string ApplicationType { get; set; } = string.Empty; // BuildingPermit, BusinessLicense, etc.
    public string ApplicationNumber { get; set; } = string.Empty; // Auto-generated unique number
    public string Status { get; set; } = string.Empty; // Draft, Submitted, UnderReview, Approved, Rejected

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FormDataJson { get; set; } = string.Empty; // Dynamic form data as JSON

    public Guid? ReviewedByUserId { get; set; }
    public string? ReviewNotes { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }

    public ICollection<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();
}

/// <summary>
/// Documents attached to an application
/// </summary>
public sealed class ApplicationDocument : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid ApplicationId { get; set; }
    public Application Application { get; set; } = null!;

    public string DocumentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }
}
