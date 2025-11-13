namespace CitizensPortal.Api.Infrastructure.TenantInitialization;

public interface ITenantInitializationService
{
    Task<(Guid TenantId, Guid AdminUserId, string InitialPassword)> CreateTenantWithAdminAsync(
        string tenantName,
        string adminName,
        string adminEmail,
        CancellationToken cancellationToken = default);

    Task SendWelcomeEmailAsync(
        string adminEmail,
        string adminName,
        string councilName,
        string initialPassword);
}
