using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Issues.GetUserIssues;

public sealed class GetUserIssues : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/issues/my", async (
            HttpContext httpContext,
            ISender sender,
            string? status,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default) =>
        {
            // Get user ID from JWT claims
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetUserIssuesQuery(userId, status, pageNumber, pageSize);
            var result = await sender.Send(query, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("GetUserIssues")
        .WithTags("Issues")
        .WithOpenApi()
        .Produces<GetUserIssuesResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Get current user's issues")
        .WithDescription("Returns a paginated list of issues created by the authenticated citizen. Optionally filter by status.");
    }
}
