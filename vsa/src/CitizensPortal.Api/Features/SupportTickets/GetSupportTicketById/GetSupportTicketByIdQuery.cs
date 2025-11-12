using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.SupportTickets.GetSupportTicketById;

public sealed record GetSupportTicketByIdQuery(
    Guid TicketId,
    Guid CitizenId
) : IRequest<ErrorOr<GetSupportTicketByIdResponse>>;
