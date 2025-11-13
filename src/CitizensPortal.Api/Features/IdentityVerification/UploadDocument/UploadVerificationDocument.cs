using Carter;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.IdentityVerification.UploadDocument;

public sealed class UploadVerificationDocument : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identity-verification/{requestId:guid}/upload",
            [Authorize] async (
                Guid requestId,
                IFormFile file,
                [FromForm] DocumentType documentType,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Get citizen ID from authenticated user
                var citizenIdClaim = user.FindFirst("CitizenId")?.Value;
                if (string.IsNullOrEmpty(citizenIdClaim) || !Guid.TryParse(citizenIdClaim, out var citizenId))
                {
                    return Results.Unauthorized();
                }

                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest(new
                    {
                        errors = new[]
                        {
                            new { code = "File.Required", description = "File is required" }
                        }
                    });
                }

                await using var stream = file.OpenReadStream();

                var command = new UploadVerificationDocumentCommand(
                    citizenId,
                    requestId,
                    documentType,
                    file.FileName,
                    file.ContentType,
                    file.Length,
                    stream
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
            .WithName("UploadVerificationDocument")
            .WithTags("IdentityVerification")
            .WithOpenApi()
            .Produces<UploadVerificationDocumentResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .DisableAntiforgery()
            .WithSummary("Upload verification document")
            .WithDescription("Uploads a document for identity verification. Accepted types: IdFront, IdBack, ProofOfAddress, Passport, TinCertificate. Max file size: 5MB. Accepted formats: JPEG, PNG, PDF.");
    }
}
