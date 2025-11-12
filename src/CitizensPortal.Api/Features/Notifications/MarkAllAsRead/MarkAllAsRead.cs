using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Notifications.MarkAllAsRead;

public sealed class MarkAllAsRead : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/notifications/read-all", async (
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new MarkAllAsReadCommand(userId);
            var result = await sender.Send(command, cancellationToken);

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
        .WithName("MarkAllNotificationsAsRead")
        .WithTags("Notifications")
        .WithOpenApi()
        .Produces<MarkAllAsReadResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Mark all notifications as read")
        .WithDescription("Marks all unread notifications for the authenticated citizen as read.");
    }
}
