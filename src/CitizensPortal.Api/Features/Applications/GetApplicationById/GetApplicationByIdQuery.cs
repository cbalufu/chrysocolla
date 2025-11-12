using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Applications.GetApplicationById;

public sealed record GetApplicationByIdQuery(
    Guid ApplicationId,
    Guid CitizenId
) : IRequest<ErrorOr<GetApplicationByIdResponse>>;
