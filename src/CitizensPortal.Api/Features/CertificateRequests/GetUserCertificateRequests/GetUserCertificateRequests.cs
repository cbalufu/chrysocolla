using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.CertificateRequests.GetUserCertificateRequests;

public sealed class GetUserCertificateRequests : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/certificate-requests/my", async (
            HttpContext httpContext,
            ISender sender,
            string? status,
            string? certificateType,
            CancellationToken cancellationToken) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetUserCertificateRequestsQuery(userId, status, certificateType);
            var result = await sender.Send(query, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("GetUserCertificateRequests")
        .WithTags("Certificate Requests")
        .WithOpenApi()
        .Produces<GetUserCertificateRequestsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithSummary("Get current user's certificate requests")
        .WithDescription("Returns all certificate requests for the authenticated citizen with optional filtering.");
    }
}
