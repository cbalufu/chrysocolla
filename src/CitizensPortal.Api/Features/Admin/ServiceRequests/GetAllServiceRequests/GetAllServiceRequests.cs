using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.ServiceRequests.GetAllServiceRequests;

public sealed class GetAllServiceRequests : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/service-requests", async (
            ISender sender,
            int page = 1,
            string? status = null,
            string? serviceType = null,
            string? priority = null,
            Guid? assignedToUserId = null,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllServiceRequestsQuery(page, status, serviceType, priority, assignedToUserId);
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
        .WithName("GetAllServiceRequests")
        .WithTags("Admin - Service Requests")
        .WithOpenApi()
        .Produces<GetAllServiceRequestsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Get all service requests (Admin/Staff)")
        .WithDescription("Returns paginated list of all service requests with optional filtering by status, service type, priority, and assigned user.");
    }
}
