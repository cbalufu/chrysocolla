using CitizensPortal.Api.Infrastructure.Database.Entities;

namespace CitizensPortal.Api.Features.IdentityVerification.UploadDocument;

public sealed record UploadVerificationDocumentResponse(
    Guid DocumentId,
    DocumentType DocumentType,
    DateTime UploadedAt,
    int TotalDocuments,
    string Message
);
