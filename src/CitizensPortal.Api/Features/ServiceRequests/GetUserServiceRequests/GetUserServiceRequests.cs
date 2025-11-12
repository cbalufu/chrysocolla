using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.ServiceRequests.GetUserServiceRequests;

public sealed class GetUserServiceRequests : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/service-requests/my", async (
            HttpContext httpContext,
            ISender sender,
            string? status,
            string? serviceType,
            CancellationToken cancellationToken) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetUserServiceRequestsQuery(userId, status, serviceType);
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
        .WithName("GetUserServiceRequests")
        .WithTags("Service Requests")
        .WithOpenApi()
        .Produces<GetUserServiceRequestsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Get current user's service requests")
        .WithDescription("Returns all service requests for the authenticated citizen. Optionally filter by status and service type.");
    }
}
