using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Notifications.GetUserNotifications;

public sealed record GetUserNotificationsQuery(
    Guid CitizenId,
    bool? IsRead,
    string? Type
) : IRequest<ErrorOr<GetUserNotificationsResponse>>;
