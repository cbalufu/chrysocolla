using CitizensPortal.Api.Infrastructure.Database.Entities;

namespace CitizensPortal.Api.Features.IdentityVerification.GetQueue;

public sealed record GetVerificationQueueResponse(
    List<VerificationQueueItemDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public sealed record VerificationQueueItemDto(
    Guid RequestId,
    string ReferenceNumber,
    string CitizenName,
    string CitizenEmail,
    NationalIdType NationalIdType,
    string NationalIdValue,
    VerificationRequestStatus Status,
    DateTime SubmittedAt,
    string? AssignedToName,
    int DocumentCount,
    int DaysWaiting
);
