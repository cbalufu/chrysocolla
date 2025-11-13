using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.CertificateRequests.GetAllCertificateRequests;

public sealed class GetAllCertificateRequestsQueryHandler
    : IRequestHandler<GetAllCertificateRequestsQuery, ErrorOr<GetAllCertificateRequestsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetAllCertificateRequestsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetAllCertificateRequestsResponse>> Handle(
        GetAllCertificateRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.CertificateRequests
            .Include(cr => cr.Citizen)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(cr => cr.Status == request.Status);
        }

        if (!string.IsNullOrEmpty(request.CertificateType))
        {
            query = query.Where(cr => cr.CertificateType == request.CertificateType);
        }

        if (request.CitizenId.HasValue)
        {
            query = query.Where(cr => cr.CitizenId == request.CitizenId.Value);
        }

        // Get total count for pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        // Apply pagination
        var requests = await query
            .OrderByDescending(cr => cr.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(cr => new AdminCertificateRequestDto(
                cr.Id,
                cr.ReferenceNumber,
                cr.CitizenId,
                $"{cr.Citizen.FirstName} {cr.Citizen.LastName}",
                cr.Citizen.Email,
                cr.CertificateType,
                cr.Status,
                cr.Purpose,
                cr.DeliveryMethod,
                cr.ApplicationFee,
                cr.IsPaid,
                cr.CreatedAt,
                cr.UpdatedAt,
                cr.ApprovalDate,
                cr.CollectionDate,
                cr.RejectionReason
            ))
            .ToListAsync(cancellationToken);

        return new GetAllCertificateRequestsResponse(
            requests,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        );
    }
}
