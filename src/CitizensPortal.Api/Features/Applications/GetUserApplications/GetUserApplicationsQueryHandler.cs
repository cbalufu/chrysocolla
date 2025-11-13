using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Applications.GetUserApplications;

public sealed class GetUserApplicationsQueryHandler
    : IRequestHandler<GetUserApplicationsQuery, ErrorOr<GetUserApplicationsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserApplicationsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserApplicationsResponse>> Handle(
        GetUserApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Applications
            .Where(a => a.CitizenId == request.CitizenId);

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

        var applications = await query
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new ApplicationDto(
                a.Id,
                a.ApplicationNumber,
                a.ApplicationType,
                a.Title,
                a.Status,
                a.Documents.Count,
                a.CreatedAt,
                a.SubmittedAt,
                a.ReviewedAt,
                a.ApprovedAt
            ))
            .ToListAsync(cancellationToken);

        return new GetUserApplicationsResponse(applications);
    }
}
