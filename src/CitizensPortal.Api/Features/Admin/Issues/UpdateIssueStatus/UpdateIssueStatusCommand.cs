using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Issues.UpdateIssueStatus;

public sealed record UpdateIssueStatusCommand(
    Guid IssueId,
    string Status
) : IRequest<ErrorOr<UpdateIssueStatusResponse>>;
