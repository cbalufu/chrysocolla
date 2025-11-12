namespace CitizensPortal.Api.Features.Issues.GetUserIssues;

public sealed record GetUserIssuesResponse(
    List<IssueDto> Issues,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public sealed record IssueDto(
    Guid Id,
    string Title,
    string Description,
    string Category,
    string Status,
    string Priority,
    string? Location,
    double? Latitude,
    double? Longitude,
    string? ImageUrls,
    int CommentCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ResolvedAt
);
