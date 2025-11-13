using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.IdentityVerification.ReviewRequest;

public sealed class ReviewVerification : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identity-verification/{requestId:guid}/review",
            [Authorize] async (
                Guid requestId,
                ReviewVerificationDto dto,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Check if user is admin or staff
                var role = user.FindFirst(ClaimTypes.Role)?.Value;
                if (role != "Admin" && role != "Staff")
                {
                    return Results.Forbid();
                }

                // Get reviewer info from claims
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var reviewerUserId))
                {
                    return Results.Unauthorized();
                }

                var reviewerName = user.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown Reviewer";

                var command = new ReviewVerificationCommand(
                    requestId,
                    reviewerUserId,
                    reviewerName,
                    dto.Approve,
                    dto.RejectionReason
                );

                var result = await sender.Send(command, cancellationToken);

                return result.Match(
                    success => Results.Ok(success),
                    errors => Results.BadRequest(new
                    {
                        errors = errors.Select(e => new
                        {
                            code = e.Code,
                            description = e.Description
                        })
                    })
                );
            })
            .WithName("ReviewVerification")
            .WithTags("IdentityVerification")
            .WithOpenApi()
            .Produces<ReviewVerificationResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithSummary("Review verification request (Admin/Staff only)")
            .WithDescription("Approves or rejects an identity verification request. When approved, creates or updates the citizen's federated profile with verified status.");
    }
}

public sealed record ReviewVerificationDto(
    bool Approve,
    string? RejectionReason
);
