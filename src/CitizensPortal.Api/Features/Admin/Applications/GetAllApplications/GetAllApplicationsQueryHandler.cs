using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.Applications.GetAllApplications;

public sealed class GetAllApplicationsQueryHandler
    : IRequestHandler<GetAllApplicationsQuery, ErrorOr<GetAllApplicationsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetAllApplicationsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetAllApplicationsResponse>> Handle(
        GetAllApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        // Build query - admin can see all applications across all citizens
        var query = _context.Applications.Include(a => a.Citizen).AsQueryable();

        // Filter by status if provided
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(a => a.Status == request.Status);
        }

        // Filter by application type if provided
        if (!string.IsNullOrEmpty(request.ApplicationType))
        {
            query = query.Where(a => a.ApplicationType == request.ApplicationType);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and get results
        var applications = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new AdminApplicationDto(
                a.Id,
                a.ApplicationNumber,
                a.CitizenId,
                $"{a.Citizen.FirstName} {a.Citizen.LastName}",
                a.ApplicationType,
                a.Title,
                a.Status,
                a.Documents.Count,
                a.CreatedAt,
                a.SubmittedAt,
                a.ReviewedAt
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetAllApplicationsResponse(
            applications,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );
    }
}
