using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.SupportTickets.CreateSupportTicket;

public sealed record CreateSupportTicketCommand(
    Guid CitizenId,
    string Subject,
    string Description,
    string Category,
    string Priority
) : IRequest<ErrorOr<CreateSupportTicketResponse>>;
