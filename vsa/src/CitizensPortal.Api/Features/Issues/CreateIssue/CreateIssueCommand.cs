using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Issues.CreateIssue;

public sealed record CreateIssueCommand(
    Guid CitizenId,
    string Title,
    string Description,
    string Category,
    string Priority,
    string? Location,
    double? Latitude,
    double? Longitude,
    string? ImageUrls
) : IRequest<ErrorOr<CreateIssueResponse>>;
