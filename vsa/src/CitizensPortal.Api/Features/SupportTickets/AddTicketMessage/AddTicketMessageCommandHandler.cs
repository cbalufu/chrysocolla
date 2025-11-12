using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.SupportTickets.AddTicketMessage;

public sealed class AddTicketMessageCommandHandler
    : IRequestHandler<AddTicketMessageCommand, ErrorOr<AddTicketMessageResponse>>
{
    private readonly ApplicationDbContext _context;

    public AddTicketMessageCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<AddTicketMessageResponse>> Handle(
        AddTicketMessageCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.SupportTickets
            .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

        if (ticket == null)
        {
            return Error.NotFound(
                code: "SupportTicket.NotFound",
                description: "Support ticket not found");
        }

        // Verify ownership
        if (ticket.CitizenId != request.SenderId)
        {
            return Error.Forbidden(
                code: "SupportTicket.Forbidden",
                description: "You are not authorized to add messages to this ticket");
        }

        var now = DateTime.UtcNow;

        var message = new TicketMessage
        {
            Id = Guid.NewGuid(),
            TicketId = request.TicketId,
            SenderId = request.SenderId,
            SenderName = request.SenderName,
            IsStaff = false, // Citizens are not staff
            Message = request.Message,
            AttachmentUrls = request.AttachmentUrls,
            CreatedAt = now
        };

        _context.TicketMessages.Add(message);

        // Update ticket timestamp
        ticket.UpdatedAt = now;

        await _context.SaveChangesAsync(cancellationToken);

        return new AddTicketMessageResponse(
            message.Id,
            message.TicketId,
            message.SenderId,
            message.SenderName,
            message.IsStaff,
            message.Message,
            message.AttachmentUrls,
            message.CreatedAt
        );
    }
}
