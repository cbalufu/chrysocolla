using Carter;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CitizensPortal.Api.Features.Auth.Refresh;

public sealed class Refresh : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async (
            [FromBody] RefreshCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                    ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .WithName("RefreshToken")
        .WithTags("Authentication")
        .WithOpenApi()
        .Produces<RefreshResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Refresh access token")
        .WithDescription("Generates a new access token using a valid refresh token. Requires a valid tenant context.");
    }
}
