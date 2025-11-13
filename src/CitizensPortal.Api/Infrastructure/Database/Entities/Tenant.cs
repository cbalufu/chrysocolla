namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a tenant in the multi-tenant system.
/// Each municipality is a separate tenant.
/// </summary>
public sealed class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string? ConnectionString { get; set; } // For future database-per-tenant migration
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Council branding configuration stored as JSON
    /// Includes logo URL, colors, fonts, custom CSS, and contact information
    /// </summary>
    public string? BrandingConfig { get; set; }

    /// <summary>
    /// Whether this council has enabled cross-council federation features
    /// </summary>
    public bool FederationEnabled { get; set; } = true;

    public Tenant() { }

    public Tenant(Guid id, string name, string subdomain)
    {
        Id = id;
        Name = name;
        Subdomain = subdomain;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
}
