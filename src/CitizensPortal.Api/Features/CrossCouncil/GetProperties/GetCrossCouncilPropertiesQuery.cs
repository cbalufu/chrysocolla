using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CrossCouncil.GetProperties;

public sealed record GetCrossCouncilPropertiesQuery(
    Guid CitizenId,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<ErrorOr<GetCrossCouncilPropertiesResponse>>;
