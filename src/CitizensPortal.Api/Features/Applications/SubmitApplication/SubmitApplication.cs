using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Applications.SubmitApplication;

public sealed class SubmitApplication : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/applications", async (
            SubmitApplicationRequest request,
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

            var command = new SubmitApplicationCommand(
                userId,
                request.ApplicationType,
                request.Title,
                request.Description,
                request.FormDataJson
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
        .WithName("SubmitApplication")
        .WithTags("Applications")
        .WithOpenApi()
        .Produces<SubmitApplicationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Submit an application")
        .WithDescription("Allows citizens to submit applications for permits, licenses, or services. FormDataJson should contain application-specific data as JSON.");
    }
}

public sealed record SubmitApplicationRequest(
    string ApplicationType,
    string Title,
    string Description,
    string FormDataJson
);
