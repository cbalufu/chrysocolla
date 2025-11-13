namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Configuration for council branding and customization.
/// Stored as JSON in Tenant.BrandingConfig
/// </summary>
public sealed class TenantBrandingConfig
{
    public string? LogoUrl { get; set; }
    public BrandingColors Colors { get; set; } = new();
    public BrandingFonts Fonts { get; set; } = new();
    public string? CustomCss { get; set; }
    public ContactInformation Contact { get; set; } = new();
}

public sealed class BrandingColors
{
    public string Primary { get; set; } = "#1976D2";
    public string Secondary { get; set; } = "#424242";
    public string Accent { get; set; } = "#FFC107";
}

public sealed class BrandingFonts
{
    public string Heading { get; set; } = "Roboto";
    public string Body { get; set; } = "Open Sans";
}

public sealed class ContactInformation
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public SocialMediaLinks SocialMedia { get; set; } = new();
}

public sealed class SocialMediaLinks
{
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
}
