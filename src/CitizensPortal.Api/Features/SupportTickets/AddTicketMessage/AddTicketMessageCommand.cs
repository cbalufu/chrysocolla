using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.SupportTickets.AddTicketMessage;

public sealed record AddTicketMessageCommand(
    Guid TicketId,
    Guid SenderId,
    string SenderName,
    string Message,
    string? AttachmentUrls
) : IRequest<ErrorOr<AddTicketMessageResponse>>;
