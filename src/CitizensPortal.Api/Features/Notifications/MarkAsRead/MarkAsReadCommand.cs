using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Notifications.MarkAsRead;

public sealed record MarkAsReadCommand(
    Guid NotificationId,
    Guid CitizenId
) : IRequest<ErrorOr<MarkAsReadResponse>>;
