using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Issues.UpdateIssue;

public sealed class UpdateIssue : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/issues/{issueId:guid}", async (
            Guid issueId,
            UpdateIssueRequest request,
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

            var command = new UpdateIssueCommand(
                issueId,
                userId,
                request.Title,
                request.Description,
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
                    ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("UpdateIssue")
        .WithTags("Issues")
        .WithOpenApi()
        .Produces<UpdateIssueResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Update an issue")
        .WithDescription("Allows citizens to update their own issues. Only non-resolved issues can be updated.");
    }
}

public sealed record UpdateIssueRequest(
    string? Title,
    string? Description,
    string? Priority,
    string? Location,
    double? Latitude,
    double? Longitude,
    string? ImageUrls
);
