using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.SupportTickets.GetSupportTicketById;

public sealed class GetSupportTicketByIdQueryHandler
    : IRequestHandler<GetSupportTicketByIdQuery, ErrorOr<GetSupportTicketByIdResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetSupportTicketByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetSupportTicketByIdResponse>> Handle(
        GetSupportTicketByIdQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.SupportTickets
            .Include(t => t.Messages)
            .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

        if (ticket == null)
        {
            return Error.NotFound(
                code: "SupportTicket.NotFound",
                description: "Support ticket not found");
        }

        // Verify ownership
        if (ticket.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "SupportTicket.Forbidden",
                description: "You are not authorized to view this support ticket");
        }

        var messages = ticket.Messages
            .OrderBy(m => m.CreatedAt)
            .Select(m => new TicketMessageDto(
                m.Id,
                m.SenderId,
                m.SenderName,
                m.IsStaff,
                m.Message,
                m.AttachmentUrls,
                m.CreatedAt
            ))
            .ToList();

        return new GetSupportTicketByIdResponse(
            ticket.Id,
            ticket.TicketNumber,
            ticket.CitizenId,
            ticket.Subject,
            ticket.Description,
            ticket.Category,
            ticket.Priority,
            ticket.Status,
            ticket.AssignedToUserId,
            ticket.AssignedAt,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            ticket.ResolvedAt,
            ticket.ClosedAt,
            messages
        );
    }
}
