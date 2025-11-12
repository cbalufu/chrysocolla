using Carter;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.CertificateRequests.UpdateRequestStatus;

public sealed class UpdateRequestStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/admin/certificate-requests/{id:guid}/status", async (
            Guid id,
            UpdateRequestStatusRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateRequestStatusCommand(
                id,
                request.Status,
                request.RejectionReason
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
        .WithName("UpdateRequestStatus")
        .WithTags("Admin - Certificate Requests")
        .WithOpenApi()
        .Produces<UpdateRequestStatusResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Update certificate request status (Admin)")
        .WithDescription("Update the status of a certificate request. Valid statuses: Submitted, UnderReview, Approved, Ready, Collected, Rejected. Rejection requires a reason. Accessible to staff and administrators.");
    }
}

public sealed record UpdateRequestStatusRequest(
    string Status,
    string? RejectionReason
);
