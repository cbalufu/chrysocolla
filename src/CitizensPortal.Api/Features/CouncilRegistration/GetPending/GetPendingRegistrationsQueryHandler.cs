using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetPending;

public sealed class GetPendingRegistrationsQueryHandler
    : IRequestHandler<GetPendingRegistrationsQuery, ErrorOr<GetPendingRegistrationsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetPendingRegistrationsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetPendingRegistrationsResponse>> Handle(
        GetPendingRegistrationsQuery request,
        CancellationToken cancellationToken)
    {
        // Build query
        var query = _context.CouncilRegistrationRequests.AsQueryable();

        // Apply status filter if provided
        if (request.Status.HasValue)
        {
            query = query.Where(r => r.Status == request.Status.Value);
        }
        else
        {
            // Default to showing requests that need action
            query = query.Where(r =>
                r.Status == CouncilRegistrationStatus.PendingApproval ||
                r.Status == CouncilRegistrationStatus.UnderReview);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting (oldest first for pending)
        query = request.Status == CouncilRegistrationStatus.PendingApproval || !request.Status.HasValue
            ? query.OrderBy(r => r.SubmittedAt)
            : query.OrderByDescending(r => r.SubmittedAt);

        // Apply pagination
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new RegistrationQueueItemDto(
                r.Id,
                r.ReferenceNumber,
                r.CouncilName,
                r.RegistrationNumber,
                r.Region,
                r.ContactEmail,
                r.AdminEmail,
                r.Status,
                r.SubmittedAt,
                CalculateDaysWaiting(r.SubmittedAt)
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetPendingRegistrationsResponse(
            items,
            request.Page,
            request.PageSize,
            totalCount,
            totalPages
        );
    }

    private static int CalculateDaysWaiting(DateTime submittedAt)
    {
        return (DateTime.UtcNow - submittedAt).Days;
    }
}
