namespace CitizensPortal.Api.Features.Admin.Issues.AddInternalComment;

public sealed record AddInternalCommentResponse(
    Guid Id,
    Guid IssueId,
    Guid UserId,
    string UserName,
    string Comment,
    bool IsInternal,
    DateTime CreatedAt
);
