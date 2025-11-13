namespace CitizensPortal.Api.Features.CrossCouncil.GetIssues;

public sealed record GetCrossCouncilIssuesResponse(
    List<AggregatedIssueDto> Issues,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public sealed record AggregatedIssueDto(
    Guid Id,
    Guid TenantId,
    string TenantName,
    Guid CitizenId,
    string Title,
    string Description,
    string Category,
    string Status,
    string Priority,
    string? Location,
    DateTime CreatedAt,
    DateTime? ResolvedAt
);
