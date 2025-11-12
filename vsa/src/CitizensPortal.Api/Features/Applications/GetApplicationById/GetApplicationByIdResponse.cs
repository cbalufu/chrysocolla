namespace CitizensPortal.Api.Features.Applications.GetApplicationById;

public sealed record GetApplicationByIdResponse(
    Guid Id,
    string ApplicationNumber,
    string ApplicationType,
    string Title,
    string Description,
    string FormDataJson,
    string Status,
    DateTime CreatedAt,
    DateTime? SubmittedAt,
    DateTime? ReviewedAt,
    DateTime? ApprovedAt,
    string? ReviewNotes,
    List<ApplicationDocumentDto> Documents
);

public sealed record ApplicationDocumentDto(
    Guid Id,
    string DocumentType,
    string FileName,
    string FileUrl,
    long FileSize,
    string ContentType,
    DateTime UploadedAt
);
