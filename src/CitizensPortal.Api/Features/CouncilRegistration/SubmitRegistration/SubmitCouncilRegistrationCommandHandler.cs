using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CouncilRegistration.SubmitRegistration;

public sealed class SubmitCouncilRegistrationCommandHandler
    : IRequestHandler<SubmitCouncilRegistrationCommand, ErrorOr<SubmitCouncilRegistrationResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SubmitCouncilRegistrationCommandHandler> _logger;

    public SubmitCouncilRegistrationCommandHandler(
        ApplicationDbContext context,
        ILogger<SubmitCouncilRegistrationCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ErrorOr<SubmitCouncilRegistrationResponse>> Handle(
        SubmitCouncilRegistrationCommand request,
        CancellationToken cancellationToken)
    {
        // Check if registration number already exists
        var existingByRegNumber = await _context.CouncilRegistrationRequests
            .Where(r => r.RegistrationNumber == request.RegistrationNumber)
            .Where(r => r.Status == CouncilRegistrationStatus.PendingApproval ||
                       r.Status == CouncilRegistrationStatus.UnderReview ||
                       r.Status == CouncilRegistrationStatus.Approved)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingByRegNumber != null)
        {
            return Error.Conflict(
                "CouncilRegistration.DuplicateRegistrationNumber",
                "A council with this registration number already exists or has a pending request");
        }

        // Check if council name already exists (approved or pending)
        var existingByName = await _context.CouncilRegistrationRequests
            .Where(r => r.CouncilName.ToLower() == request.CouncilName.ToLower())
            .Where(r => r.Status == CouncilRegistrationStatus.PendingApproval ||
                       r.Status == CouncilRegistrationStatus.UnderReview ||
                       r.Status == CouncilRegistrationStatus.Approved)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingByName != null)
        {
            return Error.Conflict(
                "CouncilRegistration.DuplicateCouncilName",
                "A council with this name already exists or has a pending request");
        }

        // Check if admin email already used
        var existingByAdminEmail = await _context.CouncilRegistrationRequests
            .Where(r => r.AdminEmail.ToLower() == request.AdminEmail.ToLower())
            .Where(r => r.Status == CouncilRegistrationStatus.Approved)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingByAdminEmail != null)
        {
            return Error.Conflict(
                "CouncilRegistration.DuplicateAdminEmail",
                "This email address is already registered");
        }

        // Generate unique reference number
        var referenceNumber = GenerateReferenceNumber();

        // Create registration request
        var registrationRequest = new CouncilRegistrationRequest
        {
            Id = Guid.NewGuid(),
            ReferenceNumber = referenceNumber,
            CouncilName = request.CouncilName,
            RegistrationNumber = request.RegistrationNumber,
            Region = request.Region,
            ContactName = request.ContactName,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            AdminName = request.AdminName,
            AdminEmail = request.AdminEmail,
            PhysicalAddress = request.PhysicalAddress,
            City = request.City,
            PostalCode = request.PostalCode,
            Status = CouncilRegistrationStatus.PendingApproval,
            SubmittedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(90) // Requests expire after 90 days
        };

        _context.CouncilRegistrationRequests.Add(registrationRequest);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Council registration request submitted: {CouncilName} (Reference: {ReferenceNumber})",
            request.CouncilName, referenceNumber);

        return new SubmitCouncilRegistrationResponse(
            registrationRequest.Id,
            referenceNumber,
            registrationRequest.SubmittedAt,
            registrationRequest.ExpiresAt!.Value,
            "Registration request submitted successfully. You will receive an email once your request has been reviewed."
        );
    }

    private static string GenerateReferenceNumber()
    {
        // Generate format: CRG-YYYYMMDD-XXXXX (e.g., CRG-20250113-B8K2M)
        var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        var randomPart = Guid.NewGuid().ToString("N")[..5].ToUpperInvariant();
        return $"CRG-{datePart}-{randomPart}";
    }
}
