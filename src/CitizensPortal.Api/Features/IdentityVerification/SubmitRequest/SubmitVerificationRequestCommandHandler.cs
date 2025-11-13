using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.FraudDetection;
using CitizensPortal.Api.Infrastructure.Notifications;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.IdentityVerification.SubmitRequest;

public sealed class SubmitVerificationRequestCommandHandler
    : IRequestHandler<SubmitVerificationRequestCommand, ErrorOr<SubmitVerificationRequestResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IVerificationNotificationService _notificationService;
    private readonly IFraudDetectionService _fraudDetectionService;
    private readonly ILogger<SubmitVerificationRequestCommandHandler> _logger;

    public SubmitVerificationRequestCommandHandler(
        ApplicationDbContext context,
        IVerificationNotificationService notificationService,
        IFraudDetectionService fraudDetectionService,
        ILogger<SubmitVerificationRequestCommandHandler> logger)
    {
        _context = context;
        _notificationService = notificationService;
        _fraudDetectionService = fraudDetectionService;
        _logger = logger;
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

        // Fraud detection: Check rate limit
        var hasExceededRateLimit = await _fraudDetectionService.HasExceededRateLimitAsync(
            request.IpAddress, 60, 3);

        if (hasExceededRateLimit)
        {
            _logger.LogWarning(
                "Rate limit exceeded for IP {IpAddress} attempting to submit verification for citizen {CitizenId}",
                request.IpAddress, request.CitizenId);

            return Error.Validation(
                "RateLimit.Exceeded",
                "Too many verification requests. Please try again later.");
        }

        // Fraud detection: Check for suspicious activity
        var isSuspicious = await _fraudDetectionService.IsSuspiciousActivityAsync(
            request.CitizenId, request.IpAddress);

        if (isSuspicious)
        {
            _logger.LogWarning(
                "Suspicious activity detected for citizen {CitizenId} from IP {IpAddress}",
                request.CitizenId, request.IpAddress);

            // Still allow the request but flag for manual review
            // In a production system, you might want to notify admins or add extra scrutiny
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

        // Record submission attempt for rate limiting
        await _fraudDetectionService.RecordSubmissionAttemptAsync(request.IpAddress);

        // Send notifications (email and in-app)
        await _notificationService.SendVerificationSubmittedEmailAsync(
            citizen.Email,
            citizen.Name,
            referenceNumber,
            verificationRequest.SubmittedAt);

        await _notificationService.CreateInAppNotificationAsync(
            citizen.TenantId!.Value,
            citizen.Id,
            "Identity Verification Submitted",
            $"Your identity verification request has been submitted. Reference: {referenceNumber}",
            "Medium");

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
