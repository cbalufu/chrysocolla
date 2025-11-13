using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Notifications.MarkAllAsRead;

public sealed record MarkAllAsReadCommand(
    Guid CitizenId
) : IRequest<ErrorOr<MarkAllAsReadResponse>>;
