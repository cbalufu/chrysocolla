using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.SupportTickets.GetUserSupportTickets;

public sealed class GetUserSupportTickets : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/support-tickets/my", async (
            HttpContext httpContext,
            ISender sender,
            string? status,
            string? category,
            CancellationToken cancellationToken) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetUserSupportTicketsQuery(userId, status, category);
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
        .WithName("GetUserSupportTickets")
        .WithTags("Support Tickets")
        .WithOpenApi()
        .Produces<GetUserSupportTicketsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Get current user's support tickets")
        .WithDescription("Returns all support tickets for the authenticated citizen. Optionally filter by status and category.");
    }
}
