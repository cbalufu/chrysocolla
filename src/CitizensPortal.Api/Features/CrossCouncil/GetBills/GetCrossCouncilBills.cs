using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.CrossCouncil.GetBills;

public sealed class GetCrossCouncilBills : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/cross-council/bills",
            [Authorize] async (
                ClaimsPrincipal user,
                ISender sender,
                string? status = null,
                int pageNumber = 1,
                int pageSize = 20,
                CancellationToken cancellationToken = default) =>
            {
                // Get citizen ID from authenticated user
                var citizenIdClaim = user.FindFirst("CitizenId")?.Value;
                if (string.IsNullOrEmpty(citizenIdClaim) || !Guid.TryParse(citizenIdClaim, out var citizenId))
                {
                    return Results.Unauthorized();
                }

                var query = new GetCrossCouncilBillsQuery(
                    citizenId,
                    status,
                    pageNumber,
                    pageSize
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
            .WithName("GetCrossCouncilBills")
            .WithTags("CrossCouncil")
            .WithOpenApi()
            .Produces<GetCrossCouncilBillsResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Get bills across all linked councils")
            .WithDescription("Retrieves an aggregated view of all bills for the citizen across all councils they are linked to. Includes total outstanding amount.");
    }
}
