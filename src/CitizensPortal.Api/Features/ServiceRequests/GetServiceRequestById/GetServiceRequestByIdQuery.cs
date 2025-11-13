using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.ServiceRequests.GetServiceRequestById;

public sealed record GetServiceRequestByIdQuery(
    Guid ServiceRequestId,
    Guid CitizenId
) : IRequest<ErrorOr<GetServiceRequestByIdResponse>>;
