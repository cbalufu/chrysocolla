namespace CitizensPortal.Api.Infrastructure.MultiTenancy;

/// <summary>
/// Marker interface for entities that support multi-tenancy.
/// Entities implementing this interface will be automatically filtered by tenant.
/// </summary>
public interface IMultiTenant
{
    Guid? TenantId { get; set; }
}
