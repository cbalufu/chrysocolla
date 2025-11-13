using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.CouncilBranding.UpdateBranding;

public sealed class UpdateCouncilBranding : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/council-branding",
            [Authorize(Policy = "StaffOrAdmin")] async (
                [FromBody] UpdateBrandingRequest request,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Get tenant ID from authenticated user's context
                var tenantIdClaim = user.FindFirst("TenantId")?.Value;
                if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out var tenantId))
                {
                    return Results.Unauthorized();
                }

                var command = new UpdateCouncilBrandingCommand(
                    tenantId,
                    request.LogoUrl,
                    request.Colors != null ? new UpdateColorsDto(
                        request.Colors.Primary,
                        request.Colors.Secondary,
                        request.Colors.Accent
                    ) : null,
                    request.Fonts != null ? new UpdateFontsDto(
                        request.Fonts.Heading,
                        request.Fonts.Body
                    ) : null,
                    request.CustomCss,
                    request.Contact != null ? new UpdateContactDto(
                        request.Contact.Phone,
                        request.Contact.Email,
                        request.Contact.Website,
                        request.Contact.Address,
                        request.Contact.SocialMedia != null ? new UpdateSocialMediaDto(
                            request.Contact.SocialMedia.Facebook,
                            request.Contact.SocialMedia.Twitter,
                            request.Contact.SocialMedia.Instagram,
                            request.Contact.SocialMedia.LinkedIn
                        ) : null
                    ) : null
                );

                var result = await sender.Send(command, cancellationToken);

                return result.Match(
                    success => Results.Ok(success),
                    errors => Results.BadRequest(new
                    {
                        errors = errors.Select(e => new
                        {
                            code = e.Code,
                            description = e.Description
                        })
                    })
                );
            })
            .WithName("UpdateCouncilBranding")
            .WithTags("CouncilBranding")
            .WithOpenApi()
            .Produces<UpdateCouncilBrandingResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Update council branding configuration")
            .WithDescription("Allows council admins to update branding including logo, colors, fonts, custom CSS, and contact information. Requires Admin or Staff role.");
    }
}

public sealed record UpdateBrandingRequest(
    string? LogoUrl,
    UpdateColorsRequest? Colors,
    UpdateFontsRequest? Fonts,
    string? CustomCss,
    UpdateContactRequest? Contact
);

public sealed record UpdateColorsRequest(
    string? Primary,
    string? Secondary,
    string? Accent
);

public sealed record UpdateFontsRequest(
    string? Heading,
    string? Body
);

public sealed record UpdateContactRequest(
    string? Phone,
    string? Email,
    string? Website,
    string? Address,
    UpdateSocialMediaRequest? SocialMedia
);

public sealed record UpdateSocialMediaRequest(
    string? Facebook,
    string? Twitter,
    string? Instagram,
    string? LinkedIn
);
