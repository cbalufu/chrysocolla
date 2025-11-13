using CitizensPortal.Api.Infrastructure.Database.Entities;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetStatus;

public sealed record GetRegistrationStatusResponse(
    Guid RequestId,
    string ReferenceNumber,
    string CouncilName,
    CouncilRegistrationStatus Status,
    DateTime SubmittedAt,
    DateTime? ReviewedAt,
    string? ReviewerName,
    string? RejectionReason,
    DateTime? ExpiresAt,
    string StatusMessage
);
