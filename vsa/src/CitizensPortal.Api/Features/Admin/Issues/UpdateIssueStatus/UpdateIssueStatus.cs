using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Issues.UpdateIssueStatus;

public sealed class UpdateIssueStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/admin/issues/{issueId:guid}/status", async (
            Guid issueId,
            UpdateIssueStatusRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateIssueStatusCommand(issueId, request.Status);
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
        .RequireAuthorization("StaffOrAdmin")
        .WithName("UpdateIssueStatus")
        .WithTags("Admin - Issues")
        .WithOpenApi()
        .Produces<UpdateIssueStatusResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Update issue status (Admin/Staff)")
        .WithDescription("Updates the status of an issue. Automatically sets ResolvedAt when status is changed to 'Resolved' or 'Closed'.");
    }
}

public sealed record UpdateIssueStatusRequest(
    string Status
);
