using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CertificateRequests.GetCertificateRequestById;

public sealed class GetCertificateRequestByIdQueryHandler
    : IRequestHandler<GetCertificateRequestByIdQuery, ErrorOr<GetCertificateRequestByIdResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetCertificateRequestByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetCertificateRequestByIdResponse>> Handle(
        GetCertificateRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var certificateRequest = await _context.CertificateRequests
            .Where(cr => cr.Id == request.RequestId)
            .Select(cr => new
            {
                cr.Id,
                cr.CitizenId,
                cr.ReferenceNumber,
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
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (certificateRequest is null)
        {
            return Error.NotFound(
                "CertificateRequest.NotFound",
                "Certificate request not found.");
        }

        // Ownership verification - citizens can only view their own requests
        if (certificateRequest.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                "CertificateRequest.Forbidden",
                "You do not have permission to view this certificate request.");
        }

        return new GetCertificateRequestByIdResponse(
            certificateRequest.Id,
            certificateRequest.ReferenceNumber,
            certificateRequest.CertificateType,
            certificateRequest.Status,
            certificateRequest.Purpose,
            certificateRequest.DeliveryMethod,
            certificateRequest.ApplicationFee,
            certificateRequest.IsPaid,
            certificateRequest.CreatedAt,
            certificateRequest.UpdatedAt,
            certificateRequest.ApprovalDate,
            certificateRequest.CollectionDate,
            certificateRequest.RejectionReason
        );
    }
}
