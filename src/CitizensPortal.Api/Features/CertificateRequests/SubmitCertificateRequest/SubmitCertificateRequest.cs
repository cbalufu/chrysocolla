using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.CertificateRequests.SubmitCertificateRequest;

public sealed class SubmitCertificateRequest : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/certificate-requests", async (
            SubmitCertificateRequestRequest request,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new SubmitCertificateRequestCommand(
                userId,
                request.CertificateType,
                request.Purpose,
                request.DeliveryMethod
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
        .WithName("SubmitCertificateRequest")
        .WithTags("Certificate Requests")
        .WithOpenApi()
        .Produces<SubmitCertificateRequestResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Submit certificate request")
        .WithDescription("Allows citizens to submit certificate requests. Types: Birth, Death, Marriage, Residence, GoodConduct, TaxClearance. Delivery: Collection, Email, Postal.");
    }
}

public sealed record SubmitCertificateRequestRequest(
    string CertificateType,
    string Purpose,
    string DeliveryMethod
);
