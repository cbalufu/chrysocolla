using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Notifications.SendCustomNotification;

public sealed record SendCustomNotificationCommand(
    Guid? CitizenId, // If null, send to all citizens in tenant
    string Type,
    string Priority,
    string Subject,
    string Message
) : IRequest<ErrorOr<SendCustomNotificationResponse>>;
