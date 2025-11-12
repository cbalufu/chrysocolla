using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.Email;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.Notifications.SendCustomNotification;

public sealed class SendCustomNotificationCommandHandler
    : IRequestHandler<SendCustomNotificationCommand, ErrorOr<SendCustomNotificationResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public SendCustomNotificationCommandHandler(
        ApplicationDbContext context,
        IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<ErrorOr<SendCustomNotificationResponse>> Handle(
        SendCustomNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var notifications = new List<Notification>();

        if (request.CitizenId.HasValue)
        {
            // Send to specific citizen
            var citizen = await _context.Citizens
                .FirstOrDefaultAsync(c => c.Id == request.CitizenId.Value, cancellationToken);

            if (citizen == null)
            {
                return Error.NotFound(
                    code: "Citizen.NotFound",
                    description: "Citizen not found");
            }

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                CitizenId = citizen.Id,
                Type = request.Type,
                Priority = request.Priority,
                Subject = request.Subject,
                Message = request.Message,
                IsRead = false,
                SentDate = now
            };

            notifications.Add(notification);

            // Send email
            await _emailService.SendEmailAsync(citizen.Email, request.Subject, request.Message, request.Priority);
        }
        else
        {
            // Send to all citizens in tenant
            var citizens = await _context.Citizens.ToListAsync(cancellationToken);

            foreach (var citizen in citizens)
            {
                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    CitizenId = citizen.Id,
                    Type = request.Type,
                    Priority = request.Priority,
                    Subject = request.Subject,
                    Message = request.Message,
                    IsRead = false,
                    SentDate = now
                };

                notifications.Add(notification);

                // Send email
                await _emailService.SendEmailAsync(citizen.Email, request.Subject, request.Message, request.Priority);
            }
        }

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync(cancellationToken);

        return new SendCustomNotificationResponse(notifications.Count);
    }
}
