using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.SupportTickets.CreateSupportTicket;

public sealed class CreateSupportTicketCommandHandler
    : IRequestHandler<CreateSupportTicketCommand, ErrorOr<CreateSupportTicketResponse>>
{
    private readonly ApplicationDbContext _context;

    public CreateSupportTicketCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<CreateSupportTicketResponse>> Handle(
        CreateSupportTicketCommand request,
        CancellationToken cancellationToken)
    {
        // Verify citizen exists
        var citizenExists = await _context.Citizens
            .AnyAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (!citizenExists)
        {
            return Error.NotFound(
                code: "Citizen.NotFound",
                description: "Citizen not found");
        }

        var now = DateTime.UtcNow;

        // Generate ticket number: TKT-{CATEGORY}-{DATE}-{GUID}
        var categoryPrefix = request.Category.ToUpper()[..3];
        var ticketNumber = $"TKT-{categoryPrefix}-{now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var ticket = new SupportTicket
        {
            Id = Guid.NewGuid(),
            CitizenId = request.CitizenId,
            TicketNumber = ticketNumber,
            Subject = request.Subject,
            Description = request.Description,
            Category = request.Category,
            Priority = request.Priority,
            Status = "Open",
            CreatedAt = now
        };

        _context.SupportTickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateSupportTicketResponse(
            ticket.Id,
            ticket.TicketNumber,
            ticket.Subject,
            ticket.Description,
            ticket.Category,
            ticket.Priority,
            ticket.Status,
            ticket.CreatedAt
        );
    }
}
