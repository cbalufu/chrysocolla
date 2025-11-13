using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.TenantInitialization;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CouncilRegistration.ReviewRegistration;

public sealed class ReviewCouncilRegistrationCommandHandler
    : IRequestHandler<ReviewCouncilRegistrationCommand, ErrorOr<ReviewCouncilRegistrationResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantInitializationService _tenantInitializationService;
    private readonly ILogger<ReviewCouncilRegistrationCommandHandler> _logger;

    public ReviewCouncilRegistrationCommandHandler(
        ApplicationDbContext context,
        ITenantInitializationService tenantInitializationService,
        ILogger<ReviewCouncilRegistrationCommandHandler> logger)
    {
        _context = context;
        _tenantInitializationService = tenantInitializationService;
        _logger = logger;
    }

    public async Task<ErrorOr<ReviewCouncilRegistrationResponse>> Handle(
        ReviewCouncilRegistrationCommand request,
        CancellationToken cancellationToken)
    {
        // Get registration request
        var registrationRequest = await _context.CouncilRegistrationRequests
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (registrationRequest == null)
        {
            return Error.NotFound(
                "CouncilRegistration.NotFound",
                "Registration request not found");
        }

        // Check if request is in valid state for review
        if (registrationRequest.Status != CouncilRegistrationStatus.PendingApproval &&
            registrationRequest.Status != CouncilRegistrationStatus.UnderReview)
        {
            return Error.Validation(
                "CouncilRegistration.InvalidStatus",
                $"Cannot review a request with status: {registrationRequest.Status}");
        }

        if (request.Approve)
        {
            // Approve registration and create tenant
            try
            {
                var (tenantId, adminUserId, initialPassword) =
                    await _tenantInitializationService.CreateTenantWithAdminAsync(
                        registrationRequest.CouncilName,
                        registrationRequest.AdminName,
                        registrationRequest.AdminEmail,
                        cancellationToken);

                // Update registration request
                registrationRequest.Status = CouncilRegistrationStatus.Approved;
                registrationRequest.ReviewedAt = DateTime.UtcNow;
                registrationRequest.ReviewedByUserId = request.ReviewerUserId;
                registrationRequest.ReviewerName = request.ReviewerName;
                registrationRequest.TenantId = tenantId;
                registrationRequest.AdminUserId = adminUserId;

                await _context.SaveChangesAsync(cancellationToken);

                // Send welcome email with credentials
                await _tenantInitializationService.SendWelcomeEmailAsync(
                    registrationRequest.AdminEmail,
                    registrationRequest.AdminName,
                    registrationRequest.CouncilName,
                    initialPassword);

                _logger.LogInformation(
                    "Council registration approved: {CouncilName} (Reference: {ReferenceNumber}, TenantId: {TenantId})",
                    registrationRequest.CouncilName, registrationRequest.ReferenceNumber, tenantId);

                return new ReviewCouncilRegistrationResponse(
                    registrationRequest.Id,
                    registrationRequest.ReferenceNumber,
                    CouncilRegistrationStatus.Approved,
                    registrationRequest.ReviewedAt!.Value,
                    request.ReviewerName,
                    null,
                    tenantId,
                    adminUserId,
                    $"Council registration approved. Tenant created successfully and credentials sent to {registrationRequest.AdminEmail}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error creating tenant for council registration {RequestId}",
                    request.RequestId);

                return Error.Failure(
                    "CouncilRegistration.TenantCreationFailed",
                    $"Failed to create tenant: {ex.Message}");
            }
        }
        else
        {
            // Reject registration
            registrationRequest.Status = CouncilRegistrationStatus.Rejected;
            registrationRequest.ReviewedAt = DateTime.UtcNow;
            registrationRequest.ReviewedByUserId = request.ReviewerUserId;
            registrationRequest.ReviewerName = request.ReviewerName;
            registrationRequest.RejectionReason = request.RejectionReason;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Council registration rejected: {CouncilName} (Reference: {ReferenceNumber})",
                registrationRequest.CouncilName, registrationRequest.ReferenceNumber);

            // TODO: Send rejection email to contact
            // await _emailService.SendEmailAsync(
            //     registrationRequest.ContactEmail,
            //     "Council Registration - Additional Information Required",
            //     $"Your registration for {registrationRequest.CouncilName} requires additional information..."
            // );

            return new ReviewCouncilRegistrationResponse(
                registrationRequest.Id,
                registrationRequest.ReferenceNumber,
                CouncilRegistrationStatus.Rejected,
                registrationRequest.ReviewedAt!.Value,
                request.ReviewerName,
                request.RejectionReason,
                null,
                null,
                "Council registration rejected. Applicant can resubmit with corrections."
            );
        }
    }
}
