using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Notifications.MarkAsRead;

public sealed class MarkAsReadCommandHandler
    : IRequestHandler<MarkAsReadCommand, ErrorOr<MarkAsReadResponse>>
{
    private readonly ApplicationDbContext _context;

    public MarkAsReadCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<MarkAsReadResponse>> Handle(
        MarkAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId, cancellationToken);

        if (notification == null)
        {
            return Error.NotFound(
                code: "Notification.NotFound",
                description: "Notification not found");
        }

        // Verify ownership
        if (notification.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "Notification.Forbidden",
                description: "You are not authorized to update this notification");
        }

        var now = DateTime.UtcNow;
        notification.IsRead = true;
        notification.ReadDate = now;

        await _context.SaveChangesAsync(cancellationToken);

        return new MarkAsReadResponse(
            notification.Id,
            notification.IsRead,
            now
        );
    }
}
