using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Notifications.MarkAllAsRead;

public sealed class MarkAllAsReadCommandHandler
    : IRequestHandler<MarkAllAsReadCommand, ErrorOr<MarkAllAsReadResponse>>
{
    private readonly ApplicationDbContext _context;

    public MarkAllAsReadCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<MarkAllAsReadResponse>> Handle(
        MarkAllAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var unreadNotifications = await _context.Notifications
            .Where(n => n.CitizenId == request.CitizenId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadDate = now;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new MarkAllAsReadResponse(unreadNotifications.Count);
    }
}
