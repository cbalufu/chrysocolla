namespace CitizensPortal.Api.Features.Admin.ServiceRequests.GetAllServiceRequests;

public sealed record GetAllServiceRequestsResponse(
    List<ServiceRequestDto> ServiceRequests,
    int TotalCount,
    int Page,
    int PageSize
);

public sealed record ServiceRequestDto(
    Guid Id,
    string RequestNumber,
    Guid CitizenId,
    string CitizenName,
    string ServiceType,
    string Title,
    string Priority,
    string Status,
    string? Location,
    DateTime? PreferredServiceDate,
    Guid? AssignedToUserId,
    DateTime CreatedAt,
    DateTime? CompletedAt
);
