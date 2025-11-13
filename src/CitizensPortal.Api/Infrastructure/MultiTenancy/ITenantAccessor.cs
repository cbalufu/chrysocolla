namespace CitizensPortal.Api.Infrastructure.MultiTenancy;

/// <summary>
/// Provides access to the current tenant context.
/// Similar to IHttpContextAccessor pattern.
/// </summary>
public interface ITenantAccessor
{
    Guid? TenantId { get; }
    void SetTenant(Guid? tenantId);
}
