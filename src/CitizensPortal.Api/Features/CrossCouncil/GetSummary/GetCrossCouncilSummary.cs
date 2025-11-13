using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.CrossCouncil.GetSummary;

public sealed class GetCrossCouncilSummary : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/cross-council/summary",
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

                var query = new GetCrossCouncilSummaryQuery(citizenId);

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
            .WithName("GetCrossCouncilSummary")
            .WithTags("CrossCouncil")
            .WithOpenApi()
            .Produces<GetCrossCouncilSummaryResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Get cross-council summary dashboard")
            .WithDescription("Retrieves a summary of all data across linked councils including counts and totals for properties, bills, and issues.");
    }
}
