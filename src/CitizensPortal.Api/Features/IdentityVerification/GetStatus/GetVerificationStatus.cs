using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.IdentityVerification.GetStatus;

public sealed class GetVerificationStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/identity-verification/status",
            [Authorize] async (
                Guid? requestId,
                string? referenceNumber,
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

                var query = new GetVerificationStatusQuery(
                    citizenId,
                    requestId,
                    referenceNumber
                );

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
            .WithName("GetVerificationStatus")
            .WithTags("IdentityVerification")
            .WithOpenApi()
            .Produces<GetVerificationStatusResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Get verification status")
            .WithDescription("Retrieves the status of an identity verification request. Can query by request ID, reference number, or retrieve the most recent request.");
    }
}
