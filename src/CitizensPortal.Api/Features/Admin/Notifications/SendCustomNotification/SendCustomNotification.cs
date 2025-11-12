using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Notifications.SendCustomNotification;

public sealed class SendCustomNotification : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/notifications", async (
            SendCustomNotificationRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new SendCustomNotificationCommand(
                request.CitizenId,
                request.Type,
                request.Priority,
                request.Subject,
                request.Message
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
        .RequireAuthorization("StaffOrAdmin")
        .WithName("SendCustomNotification")
        .WithTags("Admin - Notifications")
        .WithOpenApi()
        .Produces<SendCustomNotificationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Send custom notification (Admin/Staff)")
        .WithDescription("Allows admin/staff to send custom notifications to specific citizen or all citizens.");
    }
}

public sealed record SendCustomNotificationRequest(
    Guid? CitizenId,
    string Type,
    string Priority,
    string Subject,
    string Message
);
