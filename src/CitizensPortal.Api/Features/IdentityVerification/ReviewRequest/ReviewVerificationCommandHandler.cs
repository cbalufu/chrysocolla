using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.Notifications;
using CitizensPortal.Api.Infrastructure.Security;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.IdentityVerification.ReviewRequest;

public sealed class ReviewVerificationCommandHandler
    : IRequestHandler<ReviewVerificationCommand, ErrorOr<ReviewVerificationResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly INationalIdEncryptionService _encryptionService;
    private readonly IVerificationNotificationService _notificationService;

    public ReviewVerificationCommandHandler(
        ApplicationDbContext context,
        INationalIdEncryptionService encryptionService,
        IVerificationNotificationService notificationService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _notificationService = notificationService;
    }

    public async Task<ErrorOr<ReviewVerificationResponse>> Handle(
        ReviewVerificationCommand request,
        CancellationToken cancellationToken)
    {
        // Get verification request with citizen
        var verificationRequest = await _context.IdentityVerificationRequests
            .Include(r => r.Citizen)
            .Include(r => r.Documents.Where(d => !d.IsDeleted))
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (verificationRequest == null)
        {
            return Error.NotFound("VerificationRequest.NotFound", "Verification request not found");
        }

        // Check if request is in valid state for review
        if (verificationRequest.Status != VerificationRequestStatus.PendingReview &&
            verificationRequest.Status != VerificationRequestStatus.UnderReview)
        {
            return Error.Validation(
                "VerificationRequest.InvalidStatus",
                $"Cannot review a request with status: {verificationRequest.Status}");
        }

        // Validate that documents have been uploaded
        if (!verificationRequest.Documents.Any())
        {
            return Error.Validation(
                "VerificationRequest.NoDocuments",
                "Cannot approve a request with no documents uploaded");
        }

        var citizen = verificationRequest.Citizen;

        if (request.Approve)
        {
            // Approve verification
            verificationRequest.Status = VerificationRequestStatus.Approved;
            verificationRequest.ReviewedAt = DateTime.UtcNow;
            verificationRequest.ReviewedByUserId = request.ReviewerUserId;
            verificationRequest.ReviewerName = request.ReviewerName;

            // Update or create federated profile
            var nationalIdHash = _encryptionService.GenerateHash(verificationRequest.NationalIdValue);
            var encryptedNationalId = _encryptionService.Encrypt(verificationRequest.NationalIdValue);

            var federatedProfile = await _context.CitizenFederatedProfiles
                .FirstOrDefaultAsync(p => p.NationalIdHash == nationalIdHash, cancellationToken);

            if (federatedProfile == null)
            {
                // Create new federated profile
                federatedProfile = new CitizenFederatedProfile
                {
                    Id = Guid.NewGuid(),
                    NationalIdType = verificationRequest.NationalIdType,
                    NationalIdEncrypted = encryptedNationalId,
                    NationalIdHash = nationalIdHash,
                    VerificationStatus = VerificationStatus.Verified,
                    VerificationMethod = VerificationMethod.SelfService,
                    VerifiedAt = DateTime.UtcNow,
                    VerifiedByTenantId = citizen.TenantId,
                    ConsentStatus = ConsentStatus.NotProvided,
                    CreatedAt = DateTime.UtcNow
                };

                _context.CitizenFederatedProfiles.Add(federatedProfile);
            }
            else
            {
                // Update existing profile
                federatedProfile.VerificationStatus = VerificationStatus.Verified;
                federatedProfile.VerificationMethod = VerificationMethod.SelfService;
                federatedProfile.VerifiedAt = DateTime.UtcNow;
                federatedProfile.VerifiedByTenantId = citizen.TenantId;
                federatedProfile.UpdatedAt = DateTime.UtcNow;
            }

            // Update citizen with federated profile link
            citizen.NationalIdType = verificationRequest.NationalIdType;
            citizen.HasFederatedProfile = true;
            citizen.FederatedProfileId = federatedProfile.Id;

            await _context.SaveChangesAsync(cancellationToken);

            // Send approval notifications
            await _notificationService.SendVerificationApprovedEmailAsync(
                citizen.Email,
                citizen.Name,
                verificationRequest.ReferenceNumber,
                verificationRequest.ReviewedAt!.Value);

            await _notificationService.CreateInAppNotificationAsync(
                citizen.TenantId!.Value,
                citizen.Id,
                "Identity Verification Approved ✓",
                $"Your identity verification has been approved. You can now link accounts across councils.",
                "High");

            return new ReviewVerificationResponse(
                verificationRequest.Id,
                verificationRequest.ReferenceNumber,
                VerificationRequestStatus.Approved,
                verificationRequest.ReviewedAt!.Value,
                request.ReviewerName,
                null,
                "Verification request approved successfully. Citizen's identity has been verified."
            );
        }
        else
        {
            // Reject verification
            verificationRequest.Status = VerificationRequestStatus.Rejected;
            verificationRequest.ReviewedAt = DateTime.UtcNow;
            verificationRequest.ReviewedByUserId = request.ReviewerUserId;
            verificationRequest.ReviewerName = request.ReviewerName;
            verificationRequest.RejectionReason = request.RejectionReason;

            await _context.SaveChangesAsync(cancellationToken);

            // Send rejection notifications
            await _notificationService.SendVerificationRejectedEmailAsync(
                citizen.Email,
                citizen.Name,
                verificationRequest.ReferenceNumber,
                request.RejectionReason!,
                verificationRequest.ReviewedAt!.Value);

            await _notificationService.CreateInAppNotificationAsync(
                citizen.TenantId!.Value,
                citizen.Id,
                "Identity Verification - Additional Information Required",
                $"Your verification request needs attention. Please review the feedback and submit a new request.",
                "High");

            return new ReviewVerificationResponse(
                verificationRequest.Id,
                verificationRequest.ReferenceNumber,
                VerificationRequestStatus.Rejected,
                verificationRequest.ReviewedAt!.Value,
                request.ReviewerName,
                request.RejectionReason,
                "Verification request rejected. The citizen can submit a new request."
            );
        }
    }
}
