using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.ServiceRequests.GetAllServiceRequests;

public sealed record GetAllServiceRequestsQuery(
    int Page,
    string? Status,
    string? ServiceType,
    string? Priority,
    Guid? AssignedToUserId
) : IRequest<ErrorOr<GetAllServiceRequestsResponse>>;
