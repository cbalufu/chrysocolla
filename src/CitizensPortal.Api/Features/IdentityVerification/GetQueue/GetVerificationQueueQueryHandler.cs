using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.IdentityVerification.GetQueue;

public sealed class GetVerificationQueueQueryHandler
    : IRequestHandler<GetVerificationQueueQuery, ErrorOr<GetVerificationQueueResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetVerificationQueueQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetVerificationQueueResponse>> Handle(
        GetVerificationQueueQuery request,
        CancellationToken cancellationToken)
    {
        // Build query
        var query = _context.IdentityVerificationRequests
            .Include(r => r.Citizen)
            .Include(r => r.Documents.Where(d => !d.IsDeleted))
            .AsQueryable();

        // Apply status filter if provided
        if (request.Status.HasValue)
        {
            query = query.Where(r => r.Status == request.Status.Value);
        }
        else
        {
            // Default to showing only requests that need action
            query = query.Where(r =>
                r.Status == VerificationRequestStatus.PendingReview ||
                r.Status == VerificationRequestStatus.UnderReview);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting (oldest first for pending, newest first for others)
        query = request.Status == VerificationRequestStatus.PendingReview || !request.Status.HasValue
            ? query.OrderBy(r => r.SubmittedAt)
            : query.OrderByDescending(r => r.SubmittedAt);

        // Apply pagination
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new VerificationQueueItemDto(
                r.Id,
                r.ReferenceNumber,
                r.Citizen.Name,
                r.Citizen.Email,
                r.NationalIdType,
                r.NationalIdValue,
                r.Status,
                r.SubmittedAt,
                r.AssignedToName,
                r.Documents.Count,
                CalculateDaysWaiting(r.SubmittedAt)
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetVerificationQueueResponse(
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
