namespace CitizensPortal.Api.Features.SupportTickets.CreateSupportTicket;

public sealed record CreateSupportTicketResponse(
    Guid Id,
    string TicketNumber,
    string Subject,
    string Description,
    string Category,
    string Priority,
    string Status,
    DateTime CreatedAt
);
