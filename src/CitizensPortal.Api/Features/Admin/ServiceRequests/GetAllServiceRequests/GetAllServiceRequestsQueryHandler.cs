using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.ServiceRequests.GetAllServiceRequests;

public sealed class GetAllServiceRequestsQueryHandler
    : IRequestHandler<GetAllServiceRequestsQuery, ErrorOr<GetAllServiceRequestsResponse>>
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 20;

    public GetAllServiceRequestsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetAllServiceRequestsResponse>> Handle(
        GetAllServiceRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.ServiceRequests.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(sr => sr.Status == request.Status);
        }

        if (!string.IsNullOrEmpty(request.ServiceType))
        {
            query = query.Where(sr => sr.ServiceType == request.ServiceType);
        }

        if (!string.IsNullOrEmpty(request.Priority))
        {
            query = query.Where(sr => sr.Priority == request.Priority);
        }

        if (request.AssignedToUserId.HasValue)
        {
            query = query.Where(sr => sr.AssignedToUserId == request.AssignedToUserId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var serviceRequests = await query
            .OrderByDescending(sr => sr.CreatedAt)
            .Skip((request.Page - 1) * PageSize)
            .Take(PageSize)
            .Include(sr => sr.Citizen)
            .Select(sr => new ServiceRequestDto(
                sr.Id,
                sr.RequestNumber,
                sr.CitizenId,
                $"{sr.Citizen.FirstName} {sr.Citizen.LastName}",
                sr.ServiceType,
                sr.Title,
                sr.Priority,
                sr.Status,
                sr.Location,
                sr.PreferredServiceDate,
                sr.AssignedToUserId,
                sr.CreatedAt,
                sr.CompletedAt
            ))
            .ToListAsync(cancellationToken);

        return new GetAllServiceRequestsResponse(
            serviceRequests,
            totalCount,
            request.Page,
            PageSize
        );
    }
}
