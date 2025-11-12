namespace CitizensPortal.Api.Features.Issues.GetIssueById;

public sealed record GetIssueByIdResponse(
    Guid Id,
    Guid CitizenId,
    CitizenInfo Citizen,
    string Title,
    string Description,
    string Category,
    string Status,
    string Priority,
    string? Location,
    double? Latitude,
    double? Longitude,
    string? ImageUrls,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ResolvedAt,
    List<IssueCommentDto> Comments
);

public sealed record CitizenInfo(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);

public sealed record IssueCommentDto(
    Guid Id,
    Guid CitizenId,
    string CitizenName,
    string Comment,
    DateTime CreatedAt
);
