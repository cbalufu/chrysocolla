using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.EmergencyAlerts.GetActiveAlerts;

public sealed class GetActiveAlerts : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/emergency-alerts/active", async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetActiveAlertsQuery();
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
        .WithName("GetActiveEmergencyAlerts")
        .WithTags("Emergency Alerts")
        .WithOpenApi()
        .Produces<GetActiveAlertsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Get active emergency alerts")
        .WithDescription("Returns all active emergency alerts, ordered by severity (Critical first) and creation date.");
    }
}
