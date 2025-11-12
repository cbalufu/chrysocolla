namespace CitizensPortal.Api.Features.Admin.Issues.GetAllIssues;

public sealed record GetAllIssuesResponse(
    List<AdminIssueDto> Issues,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public sealed record AdminIssueDto(
    Guid Id,
    Guid CitizenId,
    string CitizenName,
    string Title,
    string Description,
    string Category,
    string Status,
    string Priority,
    string? Location,
    double? Latitude,
    double? Longitude,
    Guid? AssignedToUserId,
    int CommentCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ResolvedAt
);
