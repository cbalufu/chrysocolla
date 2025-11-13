using Volo.Abp.Domain.Entities;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

public sealed class VerificationDocument : Entity<Guid>
{
    public Guid VerificationRequestId { get; set; }
    public IdentityVerificationRequest VerificationRequest { get; set; } = null!;

    public DocumentType DocumentType { get; set; }

    public string BlobName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
