using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Notifications.GetUserNotifications;

public sealed class GetUserNotificationsQueryHandler
    : IRequestHandler<GetUserNotificationsQuery, ErrorOr<GetUserNotificationsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserNotificationsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserNotificationsResponse>> Handle(
        GetUserNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Notifications
            .Where(n => n.CitizenId == request.CitizenId);

        // Apply filters
        if (request.IsRead.HasValue)
        {
            query = query.Where(n => n.IsRead == request.IsRead.Value);
        }

        if (!string.IsNullOrEmpty(request.Type))
        {
            query = query.Where(n => n.Type == request.Type);
        }

        var notifications = await query
            .OrderByDescending(n => n.SentDate)
            .Select(n => new NotificationDto(
                n.Id,
                n.Type,
                n.Priority,
                n.Subject,
                n.Message,
                n.IsRead,
                n.SentDate,
                n.ReadDate
            ))
            .ToListAsync(cancellationToken);

        var unreadCount = await _context.Notifications
            .Where(n => n.CitizenId == request.CitizenId && !n.IsRead)
            .CountAsync(cancellationToken);

        return new GetUserNotificationsResponse(notifications, unreadCount);
    }
}
