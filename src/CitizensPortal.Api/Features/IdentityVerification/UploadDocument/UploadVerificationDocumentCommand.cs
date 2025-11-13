using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.IdentityVerification.UploadDocument;

public sealed record UploadVerificationDocumentCommand(
    Guid CitizenId,
    Guid RequestId,
    DocumentType DocumentType,
    string FileName,
    string ContentType,
    long FileSize,
    Stream FileStream
) : IRequest<ErrorOr<UploadVerificationDocumentResponse>>;
