using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Admin.Issues.AddInternalComment;

public sealed class AddInternalComment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/issues/{issueId:guid}/comments", async (
            Guid issueId,
            AddInternalCommentRequest request,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            // Get user ID from JWT claims
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmailClaim = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new AddInternalCommentCommand(
                issueId,
                userId,
                userEmailClaim ?? "Staff",
                request.Comment,
                request.IsInternal
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
        .RequireAuthorization("StaffOrAdmin")
        .WithName("AddInternalCommentToIssue")
        .WithTags("Admin - Issues")
        .WithOpenApi()
        .Produces<AddInternalCommentResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Add comment to issue (Admin/Staff)")
        .WithDescription("Allows staff/admin to add comments to issues. Can be marked as internal (not visible to citizens) or public.");
    }
}

public sealed record AddInternalCommentRequest(
    string Comment,
    bool IsInternal
);
