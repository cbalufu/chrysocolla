namespace CitizensPortal.Api.Features.ServiceRequests.GetUserServiceRequests;

public sealed record GetUserServiceRequestsResponse(
    List<ServiceRequestDto> ServiceRequests
);

public sealed record ServiceRequestDto(
    Guid Id,
    string RequestNumber,
    string ServiceType,
    string Title,
    string Status,
    string Priority,
    string? Location,
    DateTime? PreferredServiceDate,
    DateTime CreatedAt,
    DateTime? CompletedAt
);
