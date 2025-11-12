using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.ServiceRequests.GetUserServiceRequests;

public sealed record GetUserServiceRequestsQuery(
    Guid CitizenId,
    string? Status = null,
    string? ServiceType = null
) : IRequest<ErrorOr<GetUserServiceRequestsResponse>>;
