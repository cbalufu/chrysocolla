using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.Email;

namespace CitizensPortal.Api.Infrastructure.Notifications;

public sealed class VerificationNotificationService : IVerificationNotificationService
{
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<VerificationNotificationService> _logger;

    public VerificationNotificationService(
        IEmailService emailService,
        ApplicationDbContext context,
        ILogger<VerificationNotificationService> logger)
    {
        _emailService = emailService;
        _context = context;
        _logger = logger;
    }

    public async Task SendVerificationSubmittedEmailAsync(
        string email,
        string citizenName,
        string referenceNumber,
        DateTime submittedAt)
    {
        var subject = "Identity Verification Request Submitted";
        var body = $@"
Dear {citizenName},

Your identity verification request has been successfully submitted.

Reference Number: {referenceNumber}
Submitted: {submittedAt:yyyy-MM-dd HH:mm}

What happens next?
1. Upload your identity documents using the reference number above
2. Our team will review your documents within 3-5 business days
3. You will receive an email notification once the review is complete

Required Documents:
- Front of your ID/Passport
- Back of your ID (if applicable)
- Proof of address (utility bill or bank statement)

Please ensure all documents are clear and legible.

You can check your verification status at any time using your reference number.

Best regards,
Citizens Portal Team
";

        await _emailService.SendEmailAsync(email, subject, body, "Medium");

        _logger.LogInformation(
            "Verification submission email sent to {Email} for reference {ReferenceNumber}",
            email, referenceNumber);
    }

    public async Task SendVerificationApprovedEmailAsync(
        string email,
        string citizenName,
        string referenceNumber,
        DateTime approvedAt)
    {
        var subject = "Identity Verification Approved ✓";
        var body = $@"
Dear {citizenName},

Congratulations! Your identity verification request has been approved.

Reference Number: {referenceNumber}
Approved: {approvedAt:yyyy-MM-dd HH:mm}

Your identity has been successfully verified. You can now:
- Link your account across multiple councils
- Access services from all linked local authorities
- View aggregated data from all your councils

To link your account to other councils, log in to the Citizens Portal and navigate to your profile settings.

Thank you for completing the verification process.

Best regards,
Citizens Portal Team
";

        await _emailService.SendEmailAsync(email, subject, body, "High");

        _logger.LogInformation(
            "Verification approval email sent to {Email} for reference {ReferenceNumber}",
            email, referenceNumber);
    }

    public async Task SendVerificationRejectedEmailAsync(
        string email,
        string citizenName,
        string referenceNumber,
        string rejectionReason,
        DateTime rejectedAt)
    {
        var subject = "Identity Verification Request - Additional Information Required";
        var body = $@"
Dear {citizenName},

Thank you for submitting your identity verification request. Unfortunately, we need additional information to complete the verification.

Reference Number: {referenceNumber}
Reviewed: {rejectedAt:yyyy-MM-dd HH:mm}

Reason:
{rejectionReason}

What to do next:
1. Review the reason above
2. Submit a new verification request with the correct documents
3. Ensure all documents are clear, legible, and match the information in your profile

Common reasons for rejection:
- Documents are blurry or unclear
- Documents are expired
- Information doesn't match your profile details
- Missing required documents

If you have questions or need assistance, please contact our support team.

Best regards,
Citizens Portal Team
";

        await _emailService.SendEmailAsync(email, subject, body, "High");

        _logger.LogInformation(
            "Verification rejection email sent to {Email} for reference {ReferenceNumber}",
            email, referenceNumber);
    }

    public async Task CreateInAppNotificationAsync(
        Guid tenantId,
        Guid citizenId,
        string subject,
        string message,
        string priority = "Medium")
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CitizenId = citizenId,
            Type = "IdentityVerification",
            Priority = priority,
            Subject = subject,
            Message = message,
            IsRead = false,
            SentDate = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "In-app notification created for citizen {CitizenId}: {Subject}",
            citizenId, subject);
    }
}
