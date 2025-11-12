using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Issues.AddComment;

public sealed record AddCommentCommand(
    Guid IssueId,
    Guid CitizenId,
    string Comment
) : IRequest<ErrorOr<AddCommentResponse>>;
