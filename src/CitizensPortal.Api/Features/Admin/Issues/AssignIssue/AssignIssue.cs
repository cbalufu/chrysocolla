using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Issues.AssignIssue;

public sealed class AssignIssue : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/admin/issues/{issueId:guid}/assign", async (
            Guid issueId,
            AssignIssueRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new AssignIssueCommand(issueId, request.AssignedToUserId);
            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization("StaffOrAdmin")
        .WithName("AssignIssue")
        .WithTags("Admin - Issues")
        .WithOpenApi()
        .Produces<AssignIssueResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Assign issue to staff (Admin/Staff)")
        .WithDescription("Assigns an issue to a staff member. Automatically updates status from 'Submitted' to 'InProgress'.");
    }
}

public sealed record AssignIssueRequest(
    Guid AssignedToUserId
);
