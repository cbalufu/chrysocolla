using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Issues.GetAllIssues;

public sealed class GetAllIssues : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/issues", async (
            ISender sender,
            string? status,
            string? category,
            string? priority,
            Guid? assignedToUserId,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllIssuesQuery(
                status,
                category,
                priority,
                assignedToUserId,
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
        .WithName("GetAllIssues")
        .WithTags("Admin - Issues")
        .WithOpenApi()
        .Produces<GetAllIssuesResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Get all issues (Admin/Staff)")
        .WithDescription("Returns a paginated list of all issues across all citizens. Supports filtering by status, category, priority, and assigned user. Requires Admin or Staff role.");
    }
}
