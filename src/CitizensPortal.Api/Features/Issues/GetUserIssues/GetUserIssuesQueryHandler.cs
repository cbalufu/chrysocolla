using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Issues.GetUserIssues;

public sealed class GetUserIssuesQueryHandler
    : IRequestHandler<GetUserIssuesQuery, ErrorOr<GetUserIssuesResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserIssuesQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserIssuesResponse>> Handle(
        GetUserIssuesQuery request,
        CancellationToken cancellationToken)
    {
        // Build query
        var query = _context.Issues
            .Where(i => i.CitizenId == request.CitizenId);

        // Filter by status if provided
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(i => i.Status == request.Status);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and get results
        var issues = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(i => new IssueDto(
                i.Id,
                i.Title,
                i.Description,
                i.Category,
                i.Status,
                i.Priority,
                i.Location,
                i.Latitude,
                i.Longitude,
                i.ImageUrls,
                i.Comments.Count,
                i.CreatedAt,
                i.UpdatedAt,
                i.ResolvedAt
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetUserIssuesResponse(
            issues,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );
    }
}
