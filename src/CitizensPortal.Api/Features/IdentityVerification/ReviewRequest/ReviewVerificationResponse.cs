using CitizensPortal.Api.Infrastructure.Database.Entities;

namespace CitizensPortal.Api.Features.IdentityVerification.ReviewRequest;

public sealed record ReviewVerificationResponse(
    Guid RequestId,
    string ReferenceNumber,
    VerificationRequestStatus Status,
    DateTime ReviewedAt,
    string ReviewerName,
    string? RejectionReason,
    string Message
);
