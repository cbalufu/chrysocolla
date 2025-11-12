using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Applications.UploadDocument;

public sealed class UploadDocument : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/applications/{applicationId:guid}/documents", async (
            Guid applicationId,
            UploadDocumentRequest request,
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

            var command = new UploadDocumentCommand(
                applicationId,
                userId,
                request.DocumentType,
                request.FileName,
                request.FileUrl,
                request.FileSize,
                request.ContentType
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
        .WithName("UploadApplicationDocument")
        .WithTags("Applications")
        .WithOpenApi()
        .Produces<UploadDocumentResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Upload document to application")
        .WithDescription("Allows citizens to upload supporting documents to their applications. Maximum file size: 50MB.");
    }
}

public sealed record UploadDocumentRequest(
    string DocumentType,
    string FileName,
    string FileUrl,
    long FileSize,
    string ContentType
);
