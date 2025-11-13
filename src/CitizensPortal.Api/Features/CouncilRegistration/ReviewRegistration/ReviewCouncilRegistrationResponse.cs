using CitizensPortal.Api.Infrastructure.Database.Entities;

namespace CitizensPortal.Api.Features.CouncilRegistration.ReviewRegistration;

public sealed record ReviewCouncilRegistrationResponse(
    Guid RequestId,
    string ReferenceNumber,
    CouncilRegistrationStatus Status,
    DateTime ReviewedAt,
    string ReviewerName,
    string? RejectionReason,
    Guid? TenantId,
    Guid? AdminUserId,
    string Message
);
