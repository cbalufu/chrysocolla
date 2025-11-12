using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Issues.AddInternalComment;

public sealed record AddInternalCommentCommand(
    Guid IssueId,
    Guid UserId,
    string UserName,
    string Comment,
    bool IsInternal
) : IRequest<ErrorOr<AddInternalCommentResponse>>;
