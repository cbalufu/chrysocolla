using Carter;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CitizensPortal.Api.Features.Auth.Login;

public sealed class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            [FromBody] LoginCommand command,
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
        .WithName("Login")
        .WithTags("Authentication")
        .WithOpenApi()
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Login to the system")
        .WithDescription("Authenticates a citizen and returns JWT access and refresh tokens. Requires a valid tenant context.");
    }
}
