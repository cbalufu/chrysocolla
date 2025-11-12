using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Issues.AssignIssue;

public sealed record AssignIssueCommand(
    Guid IssueId,
    Guid AssignedToUserId
) : IRequest<ErrorOr<AssignIssueResponse>>;
