using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CertificateRequests.SubmitCertificateRequest;

public sealed class SubmitCertificateRequestCommandHandler
    : IRequestHandler<SubmitCertificateRequestCommand, ErrorOr<SubmitCertificateRequestResponse>>
{
    private readonly ApplicationDbContext _context;

    public SubmitCertificateRequestCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<SubmitCertificateRequestResponse>> Handle(
        SubmitCertificateRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Verify citizen exists
        var citizenExists = await _context.Citizens
            .AnyAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (!citizenExists)
        {
            return Error.NotFound(
                code: "Citizen.NotFound",
                description: "Citizen not found");
        }

        var now = DateTime.UtcNow;

        // Generate reference number: CERT-{TYPE}-{YYYYMMDD}-{GUID}
        var typePrefix = request.CertificateType.ToUpper()[..3];
        var referenceNumber = $"CERT-{typePrefix}-{now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        // Calculate application fee based on certificate type
        var applicationFee = CalculateFee(request.CertificateType);

        var certificateRequest = new CertificateRequest
        {
            Id = Guid.NewGuid(),
            CitizenId = request.CitizenId,
            ReferenceNumber = referenceNumber,
            CertificateType = request.CertificateType,
            Purpose = request.Purpose,
            DeliveryMethod = request.DeliveryMethod,
            Status = "Submitted",
            ApplicationFee = applicationFee,
            IsPaid = false,
            CreatedAt = now
        };

        _context.CertificateRequests.Add(certificateRequest);
        await _context.SaveChangesAsync(cancellationToken);

        return new SubmitCertificateRequestResponse(
            certificateRequest.Id,
            certificateRequest.ReferenceNumber,
            certificateRequest.CertificateType,
            certificateRequest.Purpose,
            certificateRequest.DeliveryMethod,
            certificateRequest.Status,
            certificateRequest.ApplicationFee,
            certificateRequest.IsPaid,
            certificateRequest.CreatedAt
        );
    }

    private static decimal CalculateFee(string certificateType)
    {
        return certificateType switch
        {
            "Birth" => 50.00m,
            "Death" => 50.00m,
            "Marriage" => 75.00m,
            "Residence" => 100.00m,
            "GoodConduct" => 150.00m,
            "TaxClearance" => 200.00m,
            _ => 100.00m
        };
    }
}
