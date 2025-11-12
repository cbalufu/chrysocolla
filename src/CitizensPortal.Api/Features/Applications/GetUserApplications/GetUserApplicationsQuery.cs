using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Applications.GetUserApplications;

public sealed record GetUserApplicationsQuery(
    Guid CitizenId,
    string? Status = null,
    string? ApplicationType = null
) : IRequest<ErrorOr<GetUserApplicationsResponse>>;
