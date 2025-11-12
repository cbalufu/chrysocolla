using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Bills.GetUserBills;

public sealed class GetUserBillsQueryHandler
    : IRequestHandler<GetUserBillsQuery, ErrorOr<GetUserBillsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserBillsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserBillsResponse>> Handle(
        GetUserBillsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Bills
            .Where(b => b.CitizenId == request.CitizenId);

        // Filter by status if provided
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(b => b.Status == request.Status);
        }

        var bills = await query
            .OrderByDescending(b => b.DueDate)
            .Select(b => new BillDto(
                b.Id,
                b.BillNumber,
                b.BillType,
                b.Amount,
                b.DiscountAmount,
                b.TotalAmount,
                b.Status,
                b.DueDate,
                b.CreatedAt,
                b.PaidAt
            ))
            .ToListAsync(cancellationToken);

        return new GetUserBillsResponse(bills);
    }
}
