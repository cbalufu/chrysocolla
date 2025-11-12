namespace CitizensPortal.Api.Features.Applications.GetUserApplications;

public sealed record GetUserApplicationsResponse(
    List<ApplicationDto> Applications
);

public sealed record ApplicationDto(
    Guid Id,
    string ApplicationNumber,
    string ApplicationType,
    string Title,
    string Status,
    int DocumentCount,
    DateTime CreatedAt,
    DateTime? SubmittedAt,
    DateTime? ReviewedAt,
    DateTime? ApprovedAt
);
