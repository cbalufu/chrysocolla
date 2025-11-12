using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Applications.UploadDocument;

public sealed class UploadDocumentCommandHandler
    : IRequestHandler<UploadDocumentCommand, ErrorOr<UploadDocumentResponse>>
{
    private readonly ApplicationDbContext _context;

    public UploadDocumentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UploadDocumentResponse>> Handle(
        UploadDocumentCommand request,
        CancellationToken cancellationToken)
    {
        // Verify application exists and belongs to citizen
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

        if (application == null)
        {
            return Error.NotFound(
                code: "Application.NotFound",
                description: "Application not found");
        }

        if (application.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "Application.AccessDenied",
                description: "You do not have permission to upload documents to this application");
        }

        // Create document
        var document = new ApplicationDocument
        {
            Id = Guid.NewGuid(),
            ApplicationId = request.ApplicationId,
            DocumentType = request.DocumentType,
            FileName = request.FileName,
            FileUrl = request.FileUrl,
            FileSize = request.FileSize,
            ContentType = request.ContentType,
            UploadedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.ApplicationDocuments.Add(document);
        await _context.SaveChangesAsync(cancellationToken);

        return new UploadDocumentResponse(
            document.Id,
            document.ApplicationId,
            document.DocumentType,
            document.FileName,
            document.FileUrl,
            document.FileSize,
            document.ContentType,
            document.UploadedAt
        );
    }
}
