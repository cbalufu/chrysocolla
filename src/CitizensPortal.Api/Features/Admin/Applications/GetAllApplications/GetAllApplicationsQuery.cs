using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Applications.GetAllApplications;

public sealed record GetAllApplicationsQuery(
    string? Status = null,
    string? ApplicationType = null,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<ErrorOr<GetAllApplicationsResponse>>;
