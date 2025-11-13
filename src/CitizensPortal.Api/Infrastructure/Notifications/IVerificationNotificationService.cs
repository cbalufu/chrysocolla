namespace CitizensPortal.Api.Infrastructure.Notifications;

public interface IVerificationNotificationService
{
    Task SendVerificationSubmittedEmailAsync(
        string email,
        string citizenName,
        string referenceNumber,
        DateTime submittedAt);

    Task SendVerificationApprovedEmailAsync(
        string email,
        string citizenName,
        string referenceNumber,
        DateTime approvedAt);

    Task SendVerificationRejectedEmailAsync(
        string email,
        string citizenName,
        string referenceNumber,
        string rejectionReason,
        DateTime rejectedAt);

    Task CreateInAppNotificationAsync(
        Guid tenantId,
        Guid citizenId,
        string subject,
        string message,
        string priority = "Medium");
}
