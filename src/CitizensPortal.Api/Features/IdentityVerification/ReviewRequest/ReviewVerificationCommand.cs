using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.IdentityVerification.ReviewRequest;

public sealed record ReviewVerificationCommand(
    Guid RequestId,
    Guid ReviewerUserId,
    string ReviewerName,
    bool Approve,
    string? RejectionReason
) : IRequest<ErrorOr<ReviewVerificationResponse>>;
