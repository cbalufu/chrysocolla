using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Issues.UpdateIssue;

public sealed record UpdateIssueCommand(
    Guid IssueId,
    Guid CitizenId,
    string? Title,
    string? Description,
    string? Priority,
    string? Location,
    double? Latitude,
    double? Longitude,
    string? ImageUrls
) : IRequest<ErrorOr<UpdateIssueResponse>>;
