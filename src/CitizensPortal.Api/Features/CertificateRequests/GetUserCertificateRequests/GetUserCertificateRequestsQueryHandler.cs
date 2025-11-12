using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CertificateRequests.GetUserCertificateRequests;

public sealed class GetUserCertificateRequestsQueryHandler
    : IRequestHandler<GetUserCertificateRequestsQuery, ErrorOr<GetUserCertificateRequestsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserCertificateRequestsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserCertificateRequestsResponse>> Handle(
        GetUserCertificateRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.CertificateRequests
            .Where(cr => cr.CitizenId == request.CitizenId);

        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(cr => cr.Status == request.Status);
        }

        if (!string.IsNullOrEmpty(request.CertificateType))
        {
            query = query.Where(cr => cr.CertificateType == request.CertificateType);
        }

        var requests = await query
            .OrderByDescending(cr => cr.CreatedAt)
            .Select(cr => new CertificateRequestDto(
                cr.Id,
                cr.ReferenceNumber,
                cr.CertificateType,
                cr.Status,
                cr.DeliveryMethod,
                cr.ApplicationFee,
                cr.IsPaid,
                cr.CreatedAt,
                cr.ApprovalDate,
                cr.CollectionDate
            ))
            .ToListAsync(cancellationToken);

        return new GetUserCertificateRequestsResponse(requests);
    }
}
