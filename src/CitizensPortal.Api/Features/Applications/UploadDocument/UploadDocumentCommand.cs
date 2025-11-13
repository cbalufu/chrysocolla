using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Applications.UploadDocument;

public sealed record UploadDocumentCommand(
    Guid ApplicationId,
    Guid CitizenId,
    string DocumentType,
    string FileName,
    string FileUrl,
    long FileSize,
    string ContentType
) : IRequest<ErrorOr<UploadDocumentResponse>>;
