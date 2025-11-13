using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.FederatedProfile.GetMyProfile;

public sealed class GetMyFederatedProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/federated-profile/me",
            [Authorize] async (
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

                var query = new GetMyFederatedProfileQuery(citizenId);

                var result = await sender.Send(query, cancellationToken);

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
            .WithName("GetMyFederatedProfile")
            .WithTags("FederatedProfile")
            .WithOpenApi()
            .Produces<GetMyFederatedProfileResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Get my federated profile")
            .WithDescription("Retrieves the authenticated citizen's federated profile including all linked councils.");
    }
}
