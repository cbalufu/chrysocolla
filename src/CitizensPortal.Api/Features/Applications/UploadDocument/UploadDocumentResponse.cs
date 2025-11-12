namespace CitizensPortal.Api.Features.Applications.UploadDocument;

public sealed record UploadDocumentResponse(
    Guid Id,
    Guid ApplicationId,
    string DocumentType,
    string FileName,
    string FileUrl,
    long FileSize,
    string ContentType,
    DateTime UploadedAt
);
