using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.FederatedProfile.CreateOrLink;

public sealed class CreateOrLinkFederatedProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/federated-profile/create-or-link",
            [Authorize] async (
                [FromBody] CreateOrLinkRequest request,
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

                var command = new CreateOrLinkFederatedProfileCommand(
                    citizenId,
                    request.NationalId,
                    request.NationalIdType,
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
            .WithName("CreateOrLinkFederatedProfile")
            .WithTags("FederatedProfile")
            .WithOpenApi()
            .Produces<CreateOrLinkFederatedProfileResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Create or link to a federated profile")
            .WithDescription("Creates a new federated profile or links to an existing one using a national identifier. Enables cross-council access.");
    }
}

public sealed record CreateOrLinkRequest(
    string NationalId,
    int NationalIdType,
    bool GrantConsent
);
