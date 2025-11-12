using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Applications.GetAllApplications;

public sealed class GetAllApplications : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/applications", async (
            ISender sender,
            string? status,
            string? applicationType,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllApplicationsQuery(
                status,
                applicationType,
                pageNumber,
                pageSize
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
        .WithName("GetAllApplications")
        .WithTags("Admin - Applications")
        .WithOpenApi()
        .Produces<GetAllApplicationsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Get all applications (Admin/Staff)")
        .WithDescription("Returns a paginated list of all applications across all citizens. Supports filtering by status and application type. Requires Admin or Staff role.");
    }
}
