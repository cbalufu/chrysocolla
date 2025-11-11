using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CitizensPortal.Api.Features.Citizens.Register;

public sealed class RegisterCitizen : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/citizens/register", async (
            [FromBody] RegisterCitizenCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Created($"/api/citizens/{success.Id}", success),
                errors => Results.BadRequest(new
                {
                    errors = errors.Select(e => new
                    {
                        code = e.Code,
                        description = e.Description
                    })
                })
            );
        })
        .WithName("RegisterCitizen")
        .WithTags("Citizens")
        .WithOpenApi()
        .Produces<RegisterCitizenResponse>(StatusCodes.Status201Created)
        .Produces<object>(StatusCodes.Status400BadRequest)
        .WithSummary("Register a new citizen")
        .WithDescription("Creates a new citizen registration in the system. Requires a valid tenant context.");
    }
}
