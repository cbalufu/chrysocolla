using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.SupportTickets.GetUserSupportTickets;

public sealed class GetUserSupportTicketsQueryHandler
    : IRequestHandler<GetUserSupportTicketsQuery, ErrorOr<GetUserSupportTicketsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserSupportTicketsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserSupportTicketsResponse>> Handle(
        GetUserSupportTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.SupportTickets
            .Where(t => t.CitizenId == request.CitizenId);

        // Apply filters
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(t => t.Status == request.Status);
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            query = query.Where(t => t.Category == request.Category);
        }

        var tickets = await query
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new SupportTicketDto(
                t.Id,
                t.TicketNumber,
                t.Subject,
                t.Category,
                t.Priority,
                t.Status,
                t.Messages.Count,
                t.CreatedAt,
                t.UpdatedAt,
                t.ResolvedAt
            ))
            .ToListAsync(cancellationToken);

        return new GetUserSupportTicketsResponse(tickets);
    }
}
