namespace CitizensPortal.Api.Features.Issues.AddComment;

public sealed record AddCommentResponse(
    Guid Id,
    Guid IssueId,
    Guid CitizenId,
    string CitizenName,
    string Comment,
    DateTime CreatedAt
);
