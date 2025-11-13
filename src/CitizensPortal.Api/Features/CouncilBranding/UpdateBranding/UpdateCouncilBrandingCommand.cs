using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilBranding.UpdateBranding;

public sealed record UpdateCouncilBrandingCommand(
    Guid TenantId,
    string? LogoUrl,
    UpdateColorsDto? Colors,
    UpdateFontsDto? Fonts,
    string? CustomCss,
    UpdateContactDto? Contact
) : IRequest<ErrorOr<UpdateCouncilBrandingResponse>>;

public sealed record UpdateColorsDto(
    string? Primary,
    string? Secondary,
    string? Accent
);

public sealed record UpdateFontsDto(
    string? Heading,
    string? Body
);

public sealed record UpdateContactDto(
    string? Phone,
    string? Email,
    string? Website,
    string? Address,
    UpdateSocialMediaDto? SocialMedia
);

public sealed record UpdateSocialMediaDto(
    string? Facebook,
    string? Twitter,
    string? Instagram,
    string? LinkedIn
);
