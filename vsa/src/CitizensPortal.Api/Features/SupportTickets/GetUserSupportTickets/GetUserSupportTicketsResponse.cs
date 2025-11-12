namespace CitizensPortal.Api.Features.SupportTickets.GetUserSupportTickets;

public sealed record GetUserSupportTicketsResponse(
    List<SupportTicketDto> Tickets
);

public sealed record SupportTicketDto(
    Guid Id,
    string TicketNumber,
    string Subject,
    string Category,
    string Priority,
    string Status,
    int MessageCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ResolvedAt
);
