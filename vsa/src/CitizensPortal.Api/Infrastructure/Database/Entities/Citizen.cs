using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a citizen registered in the portal.
/// Multi-tenant entity - automatically filtered by TenantId.
/// </summary>
public sealed class Citizen : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
