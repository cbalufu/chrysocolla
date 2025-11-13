namespace CitizensPortal.Api.Features.CrossCouncil.GetBills;

public sealed record GetCrossCouncilBillsResponse(
    List<AggregatedBillDto> Bills,
    decimal TotalOutstanding,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public sealed record AggregatedBillDto(
    Guid Id,
    Guid TenantId,
    string TenantName,
    Guid CitizenId,
    string BillNumber,
    string BillType,
    string Description,
    decimal Amount,
    decimal TotalAmount,
    string Status,
    DateTime DueDate,
    DateTime IssuedAt,
    DateTime? PaidAt
);
