using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.ServiceRequests.CreateServiceRequest;

public sealed class CreateServiceRequest : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/service-requests", async (
            CreateServiceRequestRequest request,
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

            var command = new CreateServiceRequestCommand(
                userId,
                request.ServiceType,
                request.Title,
                request.Description,
                request.Priority,
                request.Location,
                request.PreferredServiceDate
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
        .WithName("CreateServiceRequest")
        .WithTags("Service Requests")
        .WithOpenApi()
        .Produces<CreateServiceRequestResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Create a service request")
        .WithDescription("Allows citizens to request municipal services like waste collection, street repair, tree maintenance, etc.");
    }
}

public sealed record CreateServiceRequestRequest(
    string ServiceType,
    string Title,
    string Description,
    string Priority,
    string? Location,
    DateTime? PreferredServiceDate
);
