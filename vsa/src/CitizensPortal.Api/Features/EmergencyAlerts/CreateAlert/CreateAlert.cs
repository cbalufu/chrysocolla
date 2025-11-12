using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.EmergencyAlerts.CreateAlert;

public sealed class CreateAlert : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/emergency-alerts", async (
            CreateAlertRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateAlertCommand(
                request.AlertType,
                request.Title,
                request.Message,
                request.Severity,
                request.AffectedAreas,
                request.ExpiresAt
            );

            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        // TODO: Add role-based authorization - only admins should be able to create alerts
        // .RequireAuthorization("AdminOnly")
        .WithName("CreateEmergencyAlert")
        .WithTags("Emergency Alerts")
        .WithOpenApi()
        .Produces<CreateAlertResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Create an emergency alert")
        .WithDescription("Creates a new emergency alert. Currently requires authentication. Should be restricted to administrators in production.");
    }
}

public sealed record CreateAlertRequest(
    string AlertType,
    string Title,
    string Message,
    string Severity,
    string? AffectedAreas,
    DateTime? ExpiresAt
);
