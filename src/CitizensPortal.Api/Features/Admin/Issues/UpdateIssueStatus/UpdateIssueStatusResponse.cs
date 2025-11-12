namespace CitizensPortal.Api.Features.Admin.Issues.UpdateIssueStatus;

public sealed record UpdateIssueStatusResponse(
    Guid Id,
    string Status,
    DateTime? ResolvedAt,
    DateTime UpdatedAt
);
