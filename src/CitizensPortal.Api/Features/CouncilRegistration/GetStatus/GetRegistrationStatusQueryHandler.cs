using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetStatus;

public sealed class GetRegistrationStatusQueryHandler
    : IRequestHandler<GetRegistrationStatusQuery, ErrorOr<GetRegistrationStatusResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetRegistrationStatusQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetRegistrationStatusResponse>> Handle(
        GetRegistrationStatusQuery request,
        CancellationToken cancellationToken)
    {
        var registrationRequest = await _context.CouncilRegistrationRequests
            .FirstOrDefaultAsync(r => r.ReferenceNumber == request.ReferenceNumber, cancellationToken);

        if (registrationRequest == null)
        {
            return Error.NotFound(
                "CouncilRegistration.NotFound",
                "Registration request not found with this reference number");
        }

        return new GetRegistrationStatusResponse(
            registrationRequest.Id,
            registrationRequest.ReferenceNumber,
            registrationRequest.CouncilName,
            registrationRequest.Status,
            registrationRequest.SubmittedAt,
            registrationRequest.ReviewedAt,
            registrationRequest.ReviewerName,
            registrationRequest.RejectionReason,
            registrationRequest.ExpiresAt,
            GetStatusMessage(registrationRequest.Status, registrationRequest.ReviewedAt)
        );
    }

    private static string GetStatusMessage(CouncilRegistrationStatus status, DateTime? reviewedAt)
    {
        return status switch
        {
            CouncilRegistrationStatus.PendingApproval =>
                "Your registration request is pending approval. Our team will review your application shortly.",
            CouncilRegistrationStatus.UnderReview =>
                "Your registration request is currently under review by our team.",
            CouncilRegistrationStatus.Approved =>
                $"Your council has been approved on {reviewedAt:yyyy-MM-dd}! Login credentials have been sent to your admin email.",
            CouncilRegistrationStatus.Rejected =>
                "Your registration request was not approved. Please see the rejection reason and submit a new request if needed.",
            CouncilRegistrationStatus.Expired =>
                "Your registration request has expired. Please submit a new request.",
            _ => "Unknown status"
        };
    }
}
