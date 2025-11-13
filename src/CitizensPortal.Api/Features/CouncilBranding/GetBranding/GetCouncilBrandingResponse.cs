namespace CitizensPortal.Api.Features.CouncilBranding.GetBranding;

public sealed record GetCouncilBrandingResponse(
    Guid TenantId,
    string TenantName,
    BrandingDto Branding
);

public sealed record BrandingDto(
    string? LogoUrl,
    ColorsDto Colors,
    FontsDto Fonts,
    string? CustomCss,
    ContactDto Contact
);

public sealed record ColorsDto(
    string Primary,
    string Secondary,
    string Accent
);

public sealed record FontsDto(
    string Heading,
    string Body
);

public sealed record ContactDto(
    string? Phone,
    string? Email,
    string? Website,
    string? Address,
    SocialMediaDto SocialMedia
);

public sealed record SocialMediaDto(
    string? Facebook,
    string? Twitter,
    string? Instagram,
    string? LinkedIn
);
