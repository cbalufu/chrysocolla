namespace CitizensPortal.Api.Features.Issues.UpdateIssue;

public sealed record UpdateIssueResponse(
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
    DateTime UpdatedAt
);
