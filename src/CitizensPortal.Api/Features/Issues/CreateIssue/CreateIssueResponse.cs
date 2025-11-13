namespace CitizensPortal.Api.Features.Issues.CreateIssue;

public sealed record CreateIssueResponse(
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
    DateTime CreatedAt
);
