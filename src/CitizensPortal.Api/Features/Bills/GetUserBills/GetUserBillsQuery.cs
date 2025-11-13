using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Bills.GetUserBills;

public sealed record GetUserBillsQuery(
    Guid CitizenId,
    string? Status = null
) : IRequest<ErrorOr<GetUserBillsResponse>>;
