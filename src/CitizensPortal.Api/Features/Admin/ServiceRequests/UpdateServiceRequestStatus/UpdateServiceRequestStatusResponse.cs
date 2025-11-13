namespace CitizensPortal.Api.Features.Admin.ServiceRequests.UpdateServiceRequestStatus;

public sealed record UpdateServiceRequestStatusResponse(
    Guid Id,
    string Status,
    Guid? AssignedToUserId,
    DateTime? AssignedAt,
    DateTime? CompletedAt,
    string? CompletionNotes,
    DateTime? UpdatedAt
);
