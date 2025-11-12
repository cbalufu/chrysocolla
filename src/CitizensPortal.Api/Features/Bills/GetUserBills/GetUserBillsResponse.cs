namespace CitizensPortal.Api.Features.Bills.GetUserBills;

public sealed record GetUserBillsResponse(
    List<BillDto> Bills
);

public sealed record BillDto(
    Guid Id,
    string BillNumber,
    string BillType,
    decimal Amount,
    decimal? DiscountAmount,
    decimal TotalAmount,
    string Status,
    DateTime DueDate,
    DateTime CreatedAt,
    DateTime? PaidAt
);
