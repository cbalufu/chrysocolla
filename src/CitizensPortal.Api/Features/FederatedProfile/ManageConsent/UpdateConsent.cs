using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.FederatedProfile.ManageConsent;

public sealed class UpdateConsent : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/federated-profile/consent",
            [Authorize] async (
                [FromBody] UpdateConsentRequest request,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Get citizen ID from authenticated user
                var citizenIdClaim = user.FindFirst("CitizenId")?.Value;
                if (string.IsNullOrEmpty(citizenIdClaim) || !Guid.TryParse(citizenIdClaim, out var citizenId))
                {
                    return Results.Unauthorized();
                }

                var command = new UpdateConsentCommand(
                    citizenId,
                    request.GrantConsent
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
            .WithName("UpdateConsent")
            .WithTags("FederatedProfile")
            .WithOpenApi()
            .Produces<UpdateConsentResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Grant or revoke cross-council data sharing consent")
            .WithDescription("Allows citizens to control whether their data can be aggregated across councils (POPIA compliance).");
    }
}

public sealed record UpdateConsentRequest(
    bool GrantConsent
);
