using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

public sealed class IdentityVerificationRequest : Entity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string ReferenceNumber { get; set; } = string.Empty;

    public VerificationRequestStatus Status { get; set; } = VerificationRequestStatus.PendingReview;

    public NationalIdType NationalIdType { get; set; }
    public string NationalIdValue { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public string? ReviewerName { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime? AssignedAt { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? AssignedToName { get; set; }

    public ICollection<VerificationDocument> Documents { get; set; } = new List<VerificationDocument>();
}
