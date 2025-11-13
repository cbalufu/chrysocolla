using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.IdentityVerification.UploadDocument;

public sealed class UploadVerificationDocumentCommandHandler
    : IRequestHandler<UploadVerificationDocumentCommand, ErrorOr<UploadVerificationDocumentResponse>>
{
    private readonly ApplicationDbContext _context;

    public UploadVerificationDocumentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UploadVerificationDocumentResponse>> Handle(
        UploadVerificationDocumentCommand request,
        CancellationToken cancellationToken)
    {
        // Get verification request
        var verificationRequest = await _context.IdentityVerificationRequests
            .Include(r => r.Documents)
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (verificationRequest == null)
        {
            return Error.NotFound("VerificationRequest.NotFound", "Verification request not found");
        }

        // Verify ownership
        if (verificationRequest.CitizenId != request.CitizenId)
        {
            return Error.Forbidden("VerificationRequest.NotOwned", "You do not have access to this verification request");
        }

        // Check request is in valid state for document upload
        if (verificationRequest.Status != VerificationRequestStatus.PendingReview)
        {
            return Error.Validation(
                "VerificationRequest.InvalidStatus",
                $"Cannot upload documents for a request with status: {verificationRequest.Status}");
        }

        // Check for existing document of same type
        var existingDocument = verificationRequest.Documents
            .FirstOrDefault(d => d.DocumentType == request.DocumentType && !d.IsDeleted);

        if (existingDocument != null)
        {
            // Soft delete existing document (replace with new one)
            existingDocument.IsDeleted = true;
            existingDocument.DeletedAt = DateTime.UtcNow;
        }

        // Generate unique blob name
        var blobName = GenerateBlobName(verificationRequest.ReferenceNumber, request.DocumentType, request.FileName);

        // TODO: In production, upload to blob storage here
        // For now, we just store the metadata
        // Example: await _blobStorageService.UploadAsync(blobName, request.FileStream, request.ContentType);

        // Create document record
        var document = new VerificationDocument
        {
            Id = Guid.NewGuid(),
            VerificationRequestId = request.RequestId,
            DocumentType = request.DocumentType,
            BlobName = blobName,
            OriginalFileName = request.FileName,
            ContentType = request.ContentType,
            FileSize = request.FileSize,
            UploadedAt = DateTime.UtcNow,
            ExpiresAt = verificationRequest.ExpiresAt!.Value
        };

        _context.VerificationDocuments.Add(document);
        await _context.SaveChangesAsync(cancellationToken);

        // Count total active documents
        var totalDocuments = verificationRequest.Documents.Count(d => !d.IsDeleted) + 1;

        return new UploadVerificationDocumentResponse(
            document.Id,
            request.DocumentType,
            document.UploadedAt,
            totalDocuments,
            "Document uploaded successfully"
        );
    }

    private static string GenerateBlobName(string referenceNumber, DocumentType documentType, string fileName)
    {
        // Generate format: verification-docs/{referenceNumber}/{documentType}-{timestamp}-{guid}.ext
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var guidPart = Guid.NewGuid().ToString("N")[..8];
        var extension = Path.GetExtension(fileName);
        return $"verification-docs/{referenceNumber}/{documentType}-{timestamp}-{guidPart}{extension}";
    }
}
