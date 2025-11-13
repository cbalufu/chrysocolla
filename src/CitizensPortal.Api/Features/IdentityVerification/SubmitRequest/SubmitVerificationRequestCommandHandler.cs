using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.IdentityVerification.SubmitRequest;

public sealed class SubmitVerificationRequestCommandHandler
    : IRequestHandler<SubmitVerificationRequestCommand, ErrorOr<SubmitVerificationRequestResponse>>
{
    private readonly ApplicationDbContext _context;

    public SubmitVerificationRequestCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<SubmitVerificationRequestResponse>> Handle(
        SubmitVerificationRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Verify citizen exists
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (citizen == null)
        {
            return Error.NotFound("Citizen.NotFound", "Citizen not found");
        }

        // Check for existing pending or under review requests
        var existingRequest = await _context.IdentityVerificationRequests
            .Where(r => r.CitizenId == request.CitizenId)
            .Where(r => r.Status == VerificationRequestStatus.PendingReview ||
                       r.Status == VerificationRequestStatus.UnderReview)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingRequest != null)
        {
            return Error.Conflict(
                "VerificationRequest.AlreadyExists",
                $"You already have a verification request in progress (Reference: {existingRequest.ReferenceNumber})");
        }

        // Generate unique reference number
        var referenceNumber = GenerateReferenceNumber();

        // Create verification request
        var verificationRequest = new IdentityVerificationRequest
        {
            Id = Guid.NewGuid(),
            TenantId = citizen.TenantId,
            CitizenId = request.CitizenId,
            ReferenceNumber = referenceNumber,
            NationalIdType = request.NationalIdType,
            NationalIdValue = request.NationalIdValue,
            Status = VerificationRequestStatus.PendingReview,
            SubmittedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(90) // Documents expire after 90 days
        };

        _context.IdentityVerificationRequests.Add(verificationRequest);
        await _context.SaveChangesAsync(cancellationToken);

        return new SubmitVerificationRequestResponse(
            verificationRequest.Id,
            referenceNumber,
            verificationRequest.SubmittedAt,
            verificationRequest.ExpiresAt!.Value,
            "Verification request submitted successfully. Please upload your identity documents."
        );
    }

    private static string GenerateReferenceNumber()
    {
        // Generate format: VER-YYYYMMDD-XXXXX (e.g., VER-20250113-A7K9P)
        var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        var randomPart = Guid.NewGuid().ToString("N")[..5].ToUpperInvariant();
        return $"VER-{datePart}-{randomPart}";
    }
}
