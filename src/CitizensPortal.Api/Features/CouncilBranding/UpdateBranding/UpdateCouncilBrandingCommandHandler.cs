using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CitizensPortal.Api.Features.CouncilBranding.UpdateBranding;

public sealed class UpdateCouncilBrandingCommandHandler
    : IRequestHandler<UpdateCouncilBrandingCommand, ErrorOr<UpdateCouncilBrandingResponse>>
{
    private readonly ApplicationDbContext _context;

    public UpdateCouncilBrandingCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UpdateCouncilBrandingResponse>> Handle(
        UpdateCouncilBrandingCommand request,
        CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant == null)
        {
            return Error.NotFound("Tenant.NotFound", "Council not found");
        }

        // Get existing branding or create new
        TenantBrandingConfig brandingConfig;
        if (!string.IsNullOrWhiteSpace(tenant.BrandingConfig))
        {
            try
            {
                brandingConfig = JsonSerializer.Deserialize<TenantBrandingConfig>(tenant.BrandingConfig)
                    ?? new TenantBrandingConfig();
            }
            catch
            {
                brandingConfig = new TenantBrandingConfig();
            }
        }
        else
        {
            brandingConfig = new TenantBrandingConfig();
        }

        // Update logo if provided
        if (request.LogoUrl != null)
        {
            brandingConfig.LogoUrl = request.LogoUrl;
        }

        // Update colors if provided
        if (request.Colors != null)
        {
            if (request.Colors.Primary != null)
            {
                if (!IsValidHexColor(request.Colors.Primary))
                {
                    return Error.Validation("Colors.InvalidPrimary", "Primary color must be a valid hex color");
                }
                brandingConfig.Colors.Primary = request.Colors.Primary;
            }

            if (request.Colors.Secondary != null)
            {
                if (!IsValidHexColor(request.Colors.Secondary))
                {
                    return Error.Validation("Colors.InvalidSecondary", "Secondary color must be a valid hex color");
                }
                brandingConfig.Colors.Secondary = request.Colors.Secondary;
            }

            if (request.Colors.Accent != null)
            {
                if (!IsValidHexColor(request.Colors.Accent))
                {
                    return Error.Validation("Colors.InvalidAccent", "Accent color must be a valid hex color");
                }
                brandingConfig.Colors.Accent = request.Colors.Accent;
            }
        }

        // Update fonts if provided
        if (request.Fonts != null)
        {
            if (request.Fonts.Heading != null)
            {
                brandingConfig.Fonts.Heading = request.Fonts.Heading;
            }

            if (request.Fonts.Body != null)
            {
                brandingConfig.Fonts.Body = request.Fonts.Body;
            }
        }

        // Update custom CSS if provided (with basic sanitization)
        if (request.CustomCss != null)
        {
            // Basic sanitization - remove dangerous patterns
            var sanitizedCss = SanitizeCustomCss(request.CustomCss);
            brandingConfig.CustomCss = sanitizedCss;
        }

        // Update contact information if provided
        if (request.Contact != null)
        {
            if (request.Contact.Phone != null)
            {
                brandingConfig.Contact.Phone = request.Contact.Phone;
            }

            if (request.Contact.Email != null)
            {
                brandingConfig.Contact.Email = request.Contact.Email;
            }

            if (request.Contact.Website != null)
            {
                brandingConfig.Contact.Website = request.Contact.Website;
            }

            if (request.Contact.Address != null)
            {
                brandingConfig.Contact.Address = request.Contact.Address;
            }

            if (request.Contact.SocialMedia != null)
            {
                if (request.Contact.SocialMedia.Facebook != null)
                {
                    brandingConfig.Contact.SocialMedia.Facebook = request.Contact.SocialMedia.Facebook;
                }

                if (request.Contact.SocialMedia.Twitter != null)
                {
                    brandingConfig.Contact.SocialMedia.Twitter = request.Contact.SocialMedia.Twitter;
                }

                if (request.Contact.SocialMedia.Instagram != null)
                {
                    brandingConfig.Contact.SocialMedia.Instagram = request.Contact.SocialMedia.Instagram;
                }

                if (request.Contact.SocialMedia.LinkedIn != null)
                {
                    brandingConfig.Contact.SocialMedia.LinkedIn = request.Contact.SocialMedia.LinkedIn;
                }
            }
        }

        // Save branding config as JSON
        tenant.BrandingConfig = JsonSerializer.Serialize(brandingConfig);

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateCouncilBrandingResponse(
            tenant.Id,
            tenant.Name,
            "Council branding updated successfully"
        );
    }

    private static bool IsValidHexColor(string color)
    {
        // Validate hex color format (#RRGGBB or #RGB)
        return Regex.IsMatch(color, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
    }

    private static string SanitizeCustomCss(string css)
    {
        // Basic CSS sanitization - remove dangerous patterns
        // In production, use a proper CSS sanitization library

        // Remove @import directives
        css = Regex.Replace(css, @"@import\s+.*?;", "", RegexOptions.IgnoreCase);

        // Remove javascript: URLs
        css = Regex.Replace(css, @"javascript\s*:", "", RegexOptions.IgnoreCase);

        // Remove expression() calls (IE-specific)
        css = Regex.Replace(css, @"expression\s*\(.*?\)", "", RegexOptions.IgnoreCase);

        // Remove behavior URLs (IE-specific)
        css = Regex.Replace(css, @"behavior\s*:.*?;", "", RegexOptions.IgnoreCase);

        // Limit length to prevent abuse
        if (css.Length > 10000)
        {
            css = css.Substring(0, 10000);
        }

        return css;
    }
}
