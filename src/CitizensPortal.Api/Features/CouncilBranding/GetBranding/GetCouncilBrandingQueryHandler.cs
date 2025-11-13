using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.MultiTenancy;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CitizensPortal.Api.Features.CouncilBranding.GetBranding;

public sealed class GetCouncilBrandingQueryHandler
    : IRequestHandler<GetCouncilBrandingQuery, ErrorOr<GetCouncilBrandingResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantAccessor _tenantAccessor;

    public GetCouncilBrandingQueryHandler(
        ApplicationDbContext context,
        ITenantAccessor tenantAccessor)
    {
        _context = context;
        _tenantAccessor = tenantAccessor;
    }

    public async Task<ErrorOr<GetCouncilBrandingResponse>> Handle(
        GetCouncilBrandingQuery request,
        CancellationToken cancellationToken)
    {
        // Determine which tenant to get branding for
        var tenantId = request.TenantId ?? _tenantAccessor.TenantId;

        if (tenantId == null)
        {
            return Error.NotFound("Tenant.NotFound", "No tenant context available");
        }

        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        if (tenant == null)
        {
            return Error.NotFound("Tenant.NotFound", "Council not found");
        }

        // Parse branding config or use defaults
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

        // Map to response
        var brandingDto = new BrandingDto(
            brandingConfig.LogoUrl,
            new ColorsDto(
                brandingConfig.Colors.Primary,
                brandingConfig.Colors.Secondary,
                brandingConfig.Colors.Accent
            ),
            new FontsDto(
                brandingConfig.Fonts.Heading,
                brandingConfig.Fonts.Body
            ),
            brandingConfig.CustomCss,
            new ContactDto(
                brandingConfig.Contact.Phone,
                brandingConfig.Contact.Email,
                brandingConfig.Contact.Website,
                brandingConfig.Contact.Address,
                new SocialMediaDto(
                    brandingConfig.Contact.SocialMedia.Facebook,
                    brandingConfig.Contact.SocialMedia.Twitter,
                    brandingConfig.Contact.SocialMedia.Instagram,
                    brandingConfig.Contact.SocialMedia.LinkedIn
                )
            )
        );

        return new GetCouncilBrandingResponse(
            tenant.Id,
            tenant.Name,
            brandingDto
        );
    }
}
