using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.IdentityVerification.GetStatus;

public sealed class GetVerificationStatusQueryHandler
    : IRequestHandler<GetVerificationStatusQuery, ErrorOr<GetVerificationStatusResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetVerificationStatusQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetVerificationStatusResponse>> Handle(
        GetVerificationStatusQuery request,
        CancellationToken cancellationToken)
    {
        // Build query based on provided identifiers
        var query = _context.IdentityVerificationRequests
            .Include(r => r.Documents.Where(d => !d.IsDeleted))
            .Where(r => r.CitizenId == request.CitizenId);

        if (request.RequestId.HasValue)
        {
            query = query.Where(r => r.Id == request.RequestId.Value);
        }
        else if (!string.IsNullOrEmpty(request.ReferenceNumber))
        {
            query = query.Where(r => r.ReferenceNumber == request.ReferenceNumber);
        }
        else
        {
            // Get the most recent request if no specific identifier provided
            query = query.OrderByDescending(r => r.SubmittedAt);
        }

        var verificationRequest = await query.FirstOrDefaultAsync(cancellationToken);

        if (verificationRequest == null)
        {
            return Error.NotFound("VerificationRequest.NotFound", "Verification request not found");
        }

        // Map documents
        var documents = verificationRequest.Documents
            .Select(d => new VerificationDocumentDto(
                d.Id,
                d.DocumentType,
                d.OriginalFileName,
                d.FileSize,
                d.UploadedAt
            ))
            .ToList();

        return new GetVerificationStatusResponse(
            verificationRequest.Id,
            verificationRequest.ReferenceNumber,
            verificationRequest.Status,
            verificationRequest.NationalIdType,
            verificationRequest.SubmittedAt,
            verificationRequest.ReviewedAt,
            verificationRequest.ReviewerName,
            verificationRequest.RejectionReason,
            verificationRequest.ExpiresAt,
            verificationRequest.AssignedToName,
            documents,
            GetStatusMessage(verificationRequest.Status, verificationRequest.ReviewedAt)
        );
    }

    private static string GetStatusMessage(VerificationRequestStatus status, DateTime? reviewedAt)
    {
        return status switch
        {
            VerificationRequestStatus.PendingReview =>
                "Your verification request is pending review. Please ensure you have uploaded all required documents.",
            VerificationRequestStatus.UnderReview =>
                "Your verification request is currently under review by our team.",
            VerificationRequestStatus.Approved =>
                $"Your identity has been verified successfully on {reviewedAt:yyyy-MM-dd}.",
            VerificationRequestStatus.Rejected =>
                "Your verification request was rejected. Please see the rejection reason and submit a new request.",
            VerificationRequestStatus.Expired =>
                "Your verification request has expired. Please submit a new request.",
            _ => "Unknown status"
        };
    }
}
