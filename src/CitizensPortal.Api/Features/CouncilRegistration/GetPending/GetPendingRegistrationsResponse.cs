using CitizensPortal.Api.Infrastructure.Database.Entities;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetPending;

public sealed record GetPendingRegistrationsResponse(
    List<RegistrationQueueItemDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public sealed record RegistrationQueueItemDto(
    Guid RequestId,
    string ReferenceNumber,
    string CouncilName,
    string RegistrationNumber,
    string Region,
    string ContactEmail,
    string AdminEmail,
    CouncilRegistrationStatus Status,
    DateTime SubmittedAt,
    int DaysWaiting
);
