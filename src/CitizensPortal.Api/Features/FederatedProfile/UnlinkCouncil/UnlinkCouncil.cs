using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.FederatedProfile.UnlinkCouncil;

public sealed class UnlinkCouncil : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/federated-profile/unlink/{tenantId:guid}",
            [Authorize] async (
                Guid tenantId,
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

                var command = new UnlinkCouncilCommand(citizenId, tenantId);

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
            .WithName("UnlinkCouncil")
            .WithTags("FederatedProfile")
            .WithOpenApi()
            .Produces<UnlinkCouncilResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Unlink from a council")
            .WithDescription("Removes the link to a specific council. The council's data will no longer appear in cross-council aggregated views. Cannot unlink from your current council.");
    }
}
