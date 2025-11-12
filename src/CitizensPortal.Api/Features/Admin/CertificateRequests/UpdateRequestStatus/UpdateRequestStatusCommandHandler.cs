using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.CertificateRequests.UpdateRequestStatus;

public sealed class UpdateRequestStatusCommandHandler
    : IRequestHandler<UpdateRequestStatusCommand, ErrorOr<UpdateRequestStatusResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<UpdateRequestStatusCommand> _validator;

    public UpdateRequestStatusCommandHandler(
        ApplicationDbContext context,
        IValidator<UpdateRequestStatusCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ErrorOr<UpdateRequestStatusResponse>> Handle(
        UpdateRequestStatusCommand request,
        CancellationToken cancellationToken)
    {
        // Validate command
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Error.Validation(
                "UpdateRequestStatus.ValidationFailed",
                validationResult.Errors.First().ErrorMessage);
        }

        // Find certificate request
        var certificateRequest = await _context.CertificateRequests
            .FirstOrDefaultAsync(cr => cr.Id == request.RequestId, cancellationToken);

        if (certificateRequest is null)
        {
            return Error.NotFound(
                "CertificateRequest.NotFound",
                "Certificate request not found.");
        }

        // Update status
        certificateRequest.Status = request.Status;
        certificateRequest.UpdatedAt = DateTime.UtcNow;

        // Handle status-specific updates
        switch (request.Status)
        {
            case "Approved":
                certificateRequest.ApprovalDate = DateTime.UtcNow;
                certificateRequest.RejectionReason = null;
                break;

            case "Rejected":
                certificateRequest.RejectionReason = request.RejectionReason;
                certificateRequest.ApprovalDate = null;
                break;

            case "Collected":
                certificateRequest.CollectionDate = DateTime.UtcNow;
                break;

            default:
                // For Submitted, UnderReview, Ready - no additional fields to update
                break;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateRequestStatusResponse(
            certificateRequest.Id,
            certificateRequest.ReferenceNumber,
            certificateRequest.Status,
            certificateRequest.ApprovalDate,
            certificateRequest.CollectionDate,
            certificateRequest.RejectionReason,
            certificateRequest.UpdatedAt ?? DateTime.UtcNow
        );
    }
}
