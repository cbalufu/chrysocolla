using Carter;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CitizensPortal.Api.Infrastructure.Database;

namespace CitizensPortal.Api.Features.SupportTickets.AddTicketMessage;

public sealed class AddTicketMessage : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/support-tickets/{id:guid}/messages", async (
            Guid id,
            AddTicketMessageRequest request,
            HttpContext httpContext,
            ISender sender,
            ApplicationDbContext context,
            CancellationToken cancellationToken) =>
        {
            // Get user ID and email from JWT claims
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            // Get citizen name from database
            var citizen = await context.Citizens
                .FirstOrDefaultAsync(c => c.Id == userId, cancellationToken);

            if (citizen == null)
            {
                return Results.Unauthorized();
            }

            var senderName = $"{citizen.FirstName} {citizen.LastName}";

            var command = new AddTicketMessageCommand(
                id,
                userId,
                senderName,
                request.Message,
                request.AttachmentUrls
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
        .WithName("AddTicketMessage")
        .WithTags("Support Tickets")
        .WithOpenApi()
        .Produces<AddTicketMessageResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Add message to support ticket")
        .WithDescription("Allows citizens to add messages/replies to their support tickets.");
    }
}

public sealed record AddTicketMessageRequest(
    string Message,
    string? AttachmentUrls
);
