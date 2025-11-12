namespace CitizensPortal.Api.Features.ServiceRequests.GetServiceRequestById;

public sealed record GetServiceRequestByIdResponse(
    Guid Id,
    string RequestNumber,
    Guid CitizenId,
    string ServiceType,
    string Title,
    string Description,
    string Priority,
    string Status,
    string? Location,
    DateTime? PreferredServiceDate,
    Guid? AssignedToUserId,
    DateTime? AssignedAt,
    DateTime? CompletedAt,
    string? CompletionNotes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
