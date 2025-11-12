namespace CitizensPortal.Api.Features.Notifications.MarkAsRead;

public sealed record MarkAsReadResponse(
    Guid Id,
    bool IsRead,
    DateTime ReadDate
);
