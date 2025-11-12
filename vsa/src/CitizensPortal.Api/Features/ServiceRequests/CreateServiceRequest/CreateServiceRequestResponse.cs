namespace CitizensPortal.Api.Features.ServiceRequests.CreateServiceRequest;

public sealed record CreateServiceRequestResponse(
    Guid Id,
    string RequestNumber,
    string ServiceType,
    string Title,
    string Status,
    string Priority,
    DateTime CreatedAt
);
