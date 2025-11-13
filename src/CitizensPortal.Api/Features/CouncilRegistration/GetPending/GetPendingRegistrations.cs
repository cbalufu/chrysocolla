using Carter;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetPending;

public sealed class GetPendingRegistrations : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/council-registration/pending",
            [Authorize] async (
                CouncilRegistrationStatus? status,
                int page,
                int pageSize,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Check if user is admin or staff
                var role = user.FindFirst(ClaimTypes.Role)?.Value;
                if (role != "Admin" && role != "Staff")
                {
                    return Results.Forbid();
                }

                // Validate pagination
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 20;
                if (pageSize > 100) pageSize = 100;

                var query = new GetPendingRegistrationsQuery(
                    status,
                    page,
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
            .WithName("GetPendingRegistrations")
            .WithTags("CouncilRegistration")
            .WithOpenApi()
            .Produces<GetPendingRegistrationsResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithSummary("Get pending council registrations (Admin/Staff only)")
            .WithDescription("Retrieves a paginated list of council registration requests for admin review. Defaults to showing pending and under review requests.");
    }
}
