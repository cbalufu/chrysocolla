namespace CitizensPortal.Api.Features.Applications.SubmitApplication;

public sealed record SubmitApplicationResponse(
    Guid Id,
    string ApplicationNumber,
    string ApplicationType,
    string Title,
    string Description,
    string Status,
    DateTime CreatedAt,
    DateTime SubmittedAt
);
