using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Issues.GetAllIssues;

public sealed record GetAllIssuesQuery(
    string? Status = null,
    string? Category = null,
    string? Priority = null,
    Guid? AssignedToUserId = null,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<ErrorOr<GetAllIssuesResponse>>;
