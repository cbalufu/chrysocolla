using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Notifications.MarkAsRead;

public sealed class MarkAsRead : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/notifications/{id:guid}/read", async (
            Guid id,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new MarkAsReadCommand(id, userId);
            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("MarkNotificationAsRead")
        .WithTags("Notifications")
        .WithOpenApi()
        .Produces<MarkAsReadResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Mark notification as read")
        .WithDescription("Marks a specific notification as read.");
    }
}
