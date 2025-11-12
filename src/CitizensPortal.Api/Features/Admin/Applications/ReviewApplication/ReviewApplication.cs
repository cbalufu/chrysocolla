using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Admin.Applications.ReviewApplication;

public sealed class ReviewApplication : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/admin/applications/{applicationId:guid}/review", async (
            Guid applicationId,
            ReviewApplicationRequest request,
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

            var command = new ReviewApplicationCommand(
                applicationId,
                userId,
                request.Status,
                request.ReviewNotes
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
        .WithName("ReviewApplication")
        .WithTags("Admin - Applications")
        .WithOpenApi()
        .Produces<ReviewApplicationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Review application (Admin/Staff)")
        .WithDescription("Allows staff/admin to review and approve/reject applications. Automatically sets ReviewedAt and ApprovedAt timestamps.");
    }
}

public sealed record ReviewApplicationRequest(
    string Status,
    string? ReviewNotes
);
