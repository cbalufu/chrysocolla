using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.EmergencyAlerts.AcknowledgeAlert;

public sealed class AcknowledgeAlert : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/emergency-alerts/{alertId:guid}/acknowledge", async (
            Guid alertId,
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

            var command = new AcknowledgeAlertCommand(alertId, userId);
            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("AcknowledgeEmergencyAlert")
        .WithTags("Emergency Alerts")
        .WithOpenApi()
        .Produces<AcknowledgeAlertResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .WithSummary("Acknowledge an emergency alert")
        .WithDescription("Allows citizens to acknowledge they have seen an emergency alert. Each citizen can only acknowledge an alert once.");
    }
}
