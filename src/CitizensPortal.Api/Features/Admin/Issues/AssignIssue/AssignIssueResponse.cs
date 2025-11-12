namespace CitizensPortal.Api.Features.Admin.Issues.AssignIssue;

public sealed record AssignIssueResponse(
    Guid Id,
    Guid AssignedToUserId,
    DateTime UpdatedAt
);
