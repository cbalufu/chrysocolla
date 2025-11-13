using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.Issues.GetAllIssues;

public sealed class GetAllIssuesQueryHandler
    : IRequestHandler<GetAllIssuesQuery, ErrorOr<GetAllIssuesResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetAllIssuesQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetAllIssuesResponse>> Handle(
        GetAllIssuesQuery request,
        CancellationToken cancellationToken)
    {
        // Build query - admin can see all issues across all citizens
        var query = _context.Issues.Include(i => i.Citizen).AsQueryable();

        // Filter by status if provided
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(i => i.Status == request.Status);
        }

        // Filter by category if provided
        if (!string.IsNullOrEmpty(request.Category))
        {
            query = query.Where(i => i.Category == request.Category);
        }

        // Filter by priority if provided
        if (!string.IsNullOrEmpty(request.Priority))
        {
            query = query.Where(i => i.Priority == request.Priority);
        }

        // Filter by assigned user if provided
        if (request.AssignedToUserId.HasValue)
        {
            query = query.Where(i => i.AssignedToUserId == request.AssignedToUserId.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and get results
        var issues = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(i => new AdminIssueDto(
                i.Id,
                i.CitizenId,
                $"{i.Citizen.FirstName} {i.Citizen.LastName}",
                i.Title,
                i.Description,
                i.Category,
                i.Status,
                i.Priority,
                i.Location,
                i.Latitude,
                i.Longitude,
                i.AssignedToUserId,
                i.Comments.Count,
                i.CreatedAt,
                i.UpdatedAt,
                i.ResolvedAt
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetAllIssuesResponse(
            issues,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );
    }
}
