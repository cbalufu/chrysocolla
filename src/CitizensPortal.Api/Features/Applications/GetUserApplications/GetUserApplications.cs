using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Applications.GetUserApplications;

public sealed class GetUserApplications : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/applications/my", async (
            HttpContext httpContext,
            ISender sender,
            string? status,
            string? applicationType,
            CancellationToken cancellationToken) =>
        {
            // Get user ID from JWT claims
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetUserApplicationsQuery(userId, status, applicationType);
            var result = await sender.Send(query, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("GetUserApplications")
        .WithTags("Applications")
        .WithOpenApi()
        .Produces<GetUserApplicationsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Get current user's applications")
        .WithDescription("Returns all applications for the authenticated citizen. Optionally filter by status and application type.");
    }
}
