using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Issues.GetIssueById;

public sealed record GetIssueByIdQuery(
    Guid IssueId,
    Guid CitizenId
) : IRequest<ErrorOr<GetIssueByIdResponse>>;
