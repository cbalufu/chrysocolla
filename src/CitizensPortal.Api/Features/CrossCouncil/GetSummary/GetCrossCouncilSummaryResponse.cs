namespace CitizensPortal.Api.Features.CrossCouncil.GetSummary;

public sealed record GetCrossCouncilSummaryResponse(
    int LinkedCouncilsCount,
    List<CouncilSummaryDto> Councils,
    CrossCouncilTotalsDto Totals
);

public sealed record CouncilSummaryDto(
    Guid TenantId,
    string TenantName,
    int PropertiesCount,
    int BillsCount,
    decimal OutstandingAmount,
    int IssuesCount,
    int OpenIssuesCount
);

public sealed record CrossCouncilTotalsDto(
    int TotalProperties,
    int TotalBills,
    decimal TotalOutstanding,
    int TotalIssues,
    int TotalOpenIssues
);
