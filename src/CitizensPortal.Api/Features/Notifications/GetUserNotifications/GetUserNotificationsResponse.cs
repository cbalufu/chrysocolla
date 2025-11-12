namespace CitizensPortal.Api.Features.Notifications.GetUserNotifications;

public sealed record GetUserNotificationsResponse(
    List<NotificationDto> Notifications,
    int UnreadCount
);

public sealed record NotificationDto(
    Guid Id,
    string Type,
    string Priority,
    string Subject,
    string Message,
    bool IsRead,
    DateTime SentDate,
    DateTime? ReadDate
);
