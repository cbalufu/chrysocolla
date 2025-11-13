using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.FederatedProfile.ManageConsent;

public sealed class UpdateConsentCommandHandler
    : IRequestHandler<UpdateConsentCommand, ErrorOr<UpdateConsentResponse>>
{
    private readonly ApplicationDbContext _context;

    public UpdateConsentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UpdateConsentResponse>> Handle(
        UpdateConsentCommand request,
        CancellationToken cancellationToken)
    {
        // Get citizen
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (citizen == null)
        {
            return Error.NotFound("Citizen.NotFound", "Citizen not found");
        }

        if (!citizen.HasFederatedProfile || citizen.FederatedProfileId == null)
        {
            return Error.NotFound(
                "FederatedProfile.NotFound",
                "Citizen does not have a federated profile");
        }

        // Get federated profile
        var federatedProfile = await _context.CitizenFederatedProfiles
            .FirstOrDefaultAsync(p => p.Id == citizen.FederatedProfileId, cancellationToken);

        if (federatedProfile == null)
        {
            return Error.NotFound(
                "FederatedProfile.NotFound",
                "Federated profile not found");
        }

        // Update consent status
        var newConsentStatus = request.GrantConsent ? ConsentStatus.Granted : ConsentStatus.Revoked;

        if (federatedProfile.ConsentStatus == newConsentStatus)
        {
            return new UpdateConsentResponse(
                federatedProfile.Id,
                federatedProfile.ConsentStatus.ToString(),
                federatedProfile.ConsentChangedAt ?? federatedProfile.CreatedAt,
                "Consent status unchanged"
            );
        }

        federatedProfile.ConsentStatus = newConsentStatus;
        federatedProfile.ConsentChangedAt = DateTime.UtcNow;
        federatedProfile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var message = request.GrantConsent
            ? "Consent granted. Cross-council data sharing enabled."
            : "Consent revoked. Cross-council data sharing disabled.";

        return new UpdateConsentResponse(
            federatedProfile.Id,
            federatedProfile.ConsentStatus.ToString(),
            federatedProfile.ConsentChangedAt.Value,
            message
        );
    }
}
