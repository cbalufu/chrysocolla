using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CrossCouncil.GetSummary;

public sealed record GetCrossCouncilSummaryQuery(
    Guid CitizenId
) : IRequest<ErrorOr<GetCrossCouncilSummaryResponse>>;
