using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.CouncilRegistration.ReviewRegistration;

public sealed class ReviewCouncilRegistration : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/council-registration/{requestId:guid}/review",
            [Authorize] async (
                Guid requestId,
                ReviewCouncilRegistrationDto dto,
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

                var command = new ReviewCouncilRegistrationCommand(
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
            .WithName("ReviewCouncilRegistration")
            .WithTags("CouncilRegistration")
            .WithOpenApi()
            .Produces<ReviewCouncilRegistrationResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithSummary("Review council registration (Admin/Staff only)")
            .WithDescription("Approves or rejects a council registration request. When approved, creates a tenant and admin user, then sends credentials via email.");
    }
}

public sealed record ReviewCouncilRegistrationDto(
    bool Approve,
    string? RejectionReason
);
