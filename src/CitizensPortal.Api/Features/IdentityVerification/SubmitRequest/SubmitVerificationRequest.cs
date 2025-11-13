using Carter;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.IdentityVerification.SubmitRequest;

public sealed class SubmitVerificationRequest : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identity-verification/submit",
            [Authorize] async (
                SubmitVerificationRequestDto dto,
                ClaimsPrincipal user,
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Get citizen ID from authenticated user
                var citizenIdClaim = user.FindFirst("CitizenId")?.Value;
                if (string.IsNullOrEmpty(citizenIdClaim) || !Guid.TryParse(citizenIdClaim, out var citizenId))
                {
                    return Results.Unauthorized();
                }

                // Get IP address from HttpContext
                var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                var command = new SubmitVerificationRequestCommand(
                    citizenId,
                    dto.NationalIdType,
                    dto.NationalIdValue,
                    ipAddress
                );

                var result = await sender.Send(command, cancellationToken);

                return result.Match(
                    success => Results.Ok(success),
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
            .WithName("SubmitVerificationRequest")
            .WithTags("IdentityVerification")
            .WithOpenApi()
            .Produces<SubmitVerificationRequestResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Submit identity verification request")
            .WithDescription("Initiates an identity verification request. Returns a reference number that can be used to upload documents and track verification status.");
    }
}

public sealed record SubmitVerificationRequestDto(
    NationalIdType NationalIdType,
    string NationalIdValue
);
