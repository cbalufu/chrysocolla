using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.ServiceRequests.UpdateServiceRequestStatus;

public sealed record UpdateServiceRequestStatusCommand(
    Guid ServiceRequestId,
    string Status,
    Guid? AssignedToUserId,
    string? CompletionNotes
) : IRequest<ErrorOr<UpdateServiceRequestStatusResponse>>;
