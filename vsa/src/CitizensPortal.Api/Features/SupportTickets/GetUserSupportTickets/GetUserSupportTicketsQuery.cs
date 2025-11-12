using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.SupportTickets.GetUserSupportTickets;

public sealed record GetUserSupportTicketsQuery(
    Guid CitizenId,
    string? Status,
    string? Category
) : IRequest<ErrorOr<GetUserSupportTicketsResponse>>;
