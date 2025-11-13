using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.FederatedProfile.GetMyProfile;

public sealed class GetMyFederatedProfileQueryHandler
    : IRequestHandler<GetMyFederatedProfileQuery, ErrorOr<GetMyFederatedProfileResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetMyFederatedProfileQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetMyFederatedProfileResponse>> Handle(
        GetMyFederatedProfileQuery request,
        CancellationToken cancellationToken)
    {
        // Get citizen
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (citizen == null)
        {
            return Error.NotFound("Citizen.NotFound", "Citizen not found");
        }

        // Check if citizen has a federated profile
        if (!citizen.HasFederatedProfile || citizen.FederatedProfileId == null)
        {
            return Error.NotFound(
                "FederatedProfile.NotFound",
                "Citizen does not have a federated profile");
        }

        // Get federated profile with linked profiles
        var federatedProfile = await _context.CitizenFederatedProfiles
            .Include(p => p.LinkedProfiles)
            .FirstOrDefaultAsync(p => p.Id == citizen.FederatedProfileId, cancellationToken);

        if (federatedProfile == null)
        {
            return Error.NotFound(
                "FederatedProfile.NotFound",
                "Federated profile not found");
        }

        // Map to response
        var linkedCouncils = federatedProfile.LinkedProfiles
            .Where(l => l.IsActive)
            .Select(l => new LinkedCouncilDto(
                l.TenantId,
                l.CitizenId,
                l.LinkedAt,
                l.IsActive
            ))
            .ToList();

        return new GetMyFederatedProfileResponse(
            federatedProfile.Id,
            federatedProfile.NationalIdType.ToString(),
            federatedProfile.VerificationStatus.ToString(),
            federatedProfile.VerificationMethod?.ToString(),
            federatedProfile.VerifiedAt,
            federatedProfile.ConsentStatus.ToString(),
            federatedProfile.ConsentChangedAt,
            linkedCouncils
        );
    }
}
