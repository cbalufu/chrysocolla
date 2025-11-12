namespace CitizensPortal.Api.Features.Admin.Applications.ReviewApplication;

public sealed record ReviewApplicationResponse(
    Guid Id,
    string Status,
    Guid ReviewedByUserId,
    string? ReviewNotes,
    DateTime ReviewedAt,
    DateTime? ApprovedAt
);
