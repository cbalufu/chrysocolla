using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CrossCouncil.GetBills;

public sealed record GetCrossCouncilBillsQuery(
    Guid CitizenId,
    string? Status = null, // Filter by status (Unpaid, Paid, etc.)
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<ErrorOr<GetCrossCouncilBillsResponse>>;
