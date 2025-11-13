namespace CitizensPortal.Api.Features.SupportTickets.GetSupportTicketById;

public sealed record GetSupportTicketByIdResponse(
    Guid Id,
    string TicketNumber,
    Guid CitizenId,
    string Subject,
    string Description,
    string Category,
    string Priority,
    string Status,
    Guid? AssignedToUserId,
    DateTime? AssignedAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ResolvedAt,
    DateTime? ClosedAt,
    List<TicketMessageDto> Messages
);

public sealed record TicketMessageDto(
    Guid Id,
    Guid SenderId,
    string SenderName,
    bool IsStaff,
    string Message,
    string? AttachmentUrls,
    DateTime CreatedAt
);
