namespace CitizensPortal.Api.Features.Admin.Applications.GetAllApplications;

public sealed record GetAllApplicationsResponse(
    List<AdminApplicationDto> Applications,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public sealed record AdminApplicationDto(
    Guid Id,
    string ApplicationNumber,
    Guid CitizenId,
    string CitizenName,
    string ApplicationType,
    string Title,
    string Status,
    int DocumentCount,
    DateTime CreatedAt,
    DateTime? SubmittedAt,
    DateTime? ReviewedAt
);
