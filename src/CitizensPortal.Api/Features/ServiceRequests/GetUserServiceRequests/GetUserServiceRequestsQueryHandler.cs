using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.ServiceRequests.GetUserServiceRequests;

public sealed class GetUserServiceRequestsQueryHandler
    : IRequestHandler<GetUserServiceRequestsQuery, ErrorOr<GetUserServiceRequestsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserServiceRequestsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserServiceRequestsResponse>> Handle(
        GetUserServiceRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.ServiceRequests
            .Where(sr => sr.CitizenId == request.CitizenId);

        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(sr => sr.Status == request.Status);
        }

        if (!string.IsNullOrEmpty(request.ServiceType))
        {
            query = query.Where(sr => sr.ServiceType == request.ServiceType);
        }

        var serviceRequests = await query
            .OrderByDescending(sr => sr.CreatedAt)
            .Select(sr => new ServiceRequestDto(
                sr.Id,
                sr.RequestNumber,
                sr.ServiceType,
                sr.Title,
                sr.Status,
                sr.Priority,
                sr.Location,
                sr.PreferredServiceDate,
                sr.CreatedAt,
                sr.CompletedAt
            ))
            .ToListAsync(cancellationToken);

        return new GetUserServiceRequestsResponse(serviceRequests);
    }
}
