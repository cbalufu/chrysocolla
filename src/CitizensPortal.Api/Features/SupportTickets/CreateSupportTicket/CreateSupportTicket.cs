using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.SupportTickets.CreateSupportTicket;

public sealed class CreateSupportTicket : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/support-tickets", async (
            CreateSupportTicketRequest request,
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

            var command = new CreateSupportTicketCommand(
                userId,
                request.Subject,
                request.Description,
                request.Category,
                request.Priority
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
        .RequireAuthorization()
        .WithName("CreateSupportTicket")
        .WithTags("Support Tickets")
        .WithOpenApi()
        .Produces<CreateSupportTicketResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Create a support ticket")
        .WithDescription("Allows citizens to create support tickets for technical assistance. Categories: Technical, Account, Payment, General.");
    }
}

public sealed record CreateSupportTicketRequest(
    string Subject,
    string Description,
    string Category,
    string Priority
);
