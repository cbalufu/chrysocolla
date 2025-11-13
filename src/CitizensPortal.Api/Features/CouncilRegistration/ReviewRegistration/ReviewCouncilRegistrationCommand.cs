using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilRegistration.ReviewRegistration;

public sealed record ReviewCouncilRegistrationCommand(
    Guid RequestId,
    Guid ReviewerUserId,
    string ReviewerName,
    bool Approve,
    string? RejectionReason
) : IRequest<ErrorOr<ReviewCouncilRegistrationResponse>>;
