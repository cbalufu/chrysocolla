using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Issues.CreateIssue;

public sealed class CreateIssue : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/issues", async (
            CreateIssueRequest request,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            // Get user ID from JWT claims
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new CreateIssueCommand(
                userId,
                request.Title,
                request.Description,
                request.Category,
                request.Priority,
                request.Location,
                request.Latitude,
                request.Longitude,
                request.ImageUrls
            );

            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("CreateIssue")
        .WithTags("Issues")
        .WithOpenApi()
        .Produces<CreateIssueResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Create a new issue")
        .WithDescription("Allows authenticated citizens to report a new community issue.");
    }
}

public sealed record CreateIssueRequest(
    string Title,
    string Description,
    string Category,
    string Priority,
    string? Location,
    double? Latitude,
    double? Longitude,
    string? ImageUrls
);
