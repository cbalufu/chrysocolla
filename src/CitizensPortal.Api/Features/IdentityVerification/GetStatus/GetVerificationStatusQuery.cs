using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.IdentityVerification.GetStatus;

public sealed record GetVerificationStatusQuery(
    Guid CitizenId,
    Guid? RequestId,
    string? ReferenceNumber
) : IRequest<ErrorOr<GetVerificationStatusResponse>>;
