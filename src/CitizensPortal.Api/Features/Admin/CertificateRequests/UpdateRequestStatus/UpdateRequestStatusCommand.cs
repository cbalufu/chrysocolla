using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.CertificateRequests.UpdateRequestStatus;

public sealed record UpdateRequestStatusCommand(
    Guid RequestId,
    string Status,
    string? RejectionReason
) : IRequest<ErrorOr<UpdateRequestStatusResponse>>;
