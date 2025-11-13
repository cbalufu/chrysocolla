using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CrossCouncil.GetIssues;

public sealed record GetCrossCouncilIssuesQuery(
    Guid CitizenId,
    string? Status = null, // Filter by status (Reported, InProgress, Resolved, etc.)
    string? Category = null, // Filter by category
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<ErrorOr<GetCrossCouncilIssuesResponse>>;
