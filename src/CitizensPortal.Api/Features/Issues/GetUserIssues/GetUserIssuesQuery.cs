using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Issues.GetUserIssues;

public sealed record GetUserIssuesQuery(
    Guid CitizenId,
    string? Status = null,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<ErrorOr<GetUserIssuesResponse>>;
