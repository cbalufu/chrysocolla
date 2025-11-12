using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.CertificateRequests.GetAllCertificateRequests;

public sealed class GetAllCertificateRequests : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/certificate-requests", async (
            ISender sender,
            int page = 1,
            int pageSize = 10,
            string? status = null,
            string? certificateType = null,
            Guid? citizenId = null,
            CancellationToken cancellationToken = default) =>
        {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = new GetAllCertificateRequestsQuery(
                page,
                pageSize,
                status,
                certificateType,
                citizenId
            );

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
        .RequireAuthorization("StaffOrAdmin")
        .WithName("GetAllCertificateRequests")
        .WithTags("Admin - Certificate Requests")
        .WithOpenApi()
        .Produces<GetAllCertificateRequestsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Get all certificate requests (Admin)")
        .WithDescription("Returns paginated list of all certificate requests with optional filtering by status, certificate type, or citizen. Accessible to staff and administrators.");
    }
}
