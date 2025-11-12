namespace CitizensPortal.Api.Features.SupportTickets.AddTicketMessage;

public sealed record AddTicketMessageResponse(
    Guid Id,
    Guid TicketId,
    Guid SenderId,
    string SenderName,
    bool IsStaff,
    string Message,
    string? AttachmentUrls,
    DateTime CreatedAt
);
