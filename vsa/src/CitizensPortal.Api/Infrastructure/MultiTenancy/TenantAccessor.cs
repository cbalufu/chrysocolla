namespace CitizensPortal.Api.Infrastructure.MultiTenancy;

/// <summary>
/// Thread-safe accessor for the current tenant context.
/// Uses AsyncLocal for proper async/await flow.
/// </summary>
public sealed class TenantAccessor : ITenantAccessor
{
    private static readonly AsyncLocal<Guid?> _currentTenant = new();

    public Guid? TenantId => _currentTenant.Value;

    public void SetTenant(Guid? tenantId)
    {
        _currentTenant.Value = tenantId;
    }
}
