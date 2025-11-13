using CitizensPortal.Api.Infrastructure.Database.Entities;

namespace CitizensPortal.Api.Features.IdentityVerification.GetStatus;

public sealed record GetVerificationStatusResponse(
    Guid RequestId,
    string ReferenceNumber,
    VerificationRequestStatus Status,
    NationalIdType NationalIdType,
    DateTime SubmittedAt,
    DateTime? ReviewedAt,
    string? ReviewerName,
    string? RejectionReason,
    DateTime? ExpiresAt,
    string? AssignedToName,
    List<VerificationDocumentDto> Documents,
    string StatusMessage
);

public sealed record VerificationDocumentDto(
    Guid DocumentId,
    DocumentType DocumentType,
    string FileName,
    long FileSize,
    DateTime UploadedAt
);
