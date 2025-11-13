using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.Security;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.FederatedProfile.CreateOrLink;

public sealed class CreateOrLinkFederatedProfileCommandHandler
    : IRequestHandler<CreateOrLinkFederatedProfileCommand, ErrorOr<CreateOrLinkFederatedProfileResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly INationalIdEncryptionService _encryptionService;

    public CreateOrLinkFederatedProfileCommandHandler(
        ApplicationDbContext context,
        INationalIdEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task<ErrorOr<CreateOrLinkFederatedProfileResponse>> Handle(
        CreateOrLinkFederatedProfileCommand request,
        CancellationToken cancellationToken)
    {
        // Verify citizen exists
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (citizen == null)
        {
            return Error.NotFound("Citizen.NotFound", "Citizen not found");
        }

        // Check if citizen already has a federated profile
        if (citizen.HasFederatedProfile && citizen.FederatedProfileId != null)
        {
            return Error.Conflict(
                "Citizen.AlreadyLinked",
                "Citizen is already linked to a federated profile");
        }

        // Encrypt and hash the national ID
        string encryptedNationalId;
        string nationalIdHash;

        try
        {
            encryptedNationalId = _encryptionService.Encrypt(request.NationalId);
            nationalIdHash = _encryptionService.GenerateHash(request.NationalId);
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Encryption.Failed",
                $"Failed to encrypt national ID: {ex.Message}");
        }

        // Check if a federated profile already exists for this national ID
        var existingProfile = await _context.CitizenFederatedProfiles
            .Include(p => p.LinkedProfiles)
            .FirstOrDefaultAsync(p => p.NationalIdHash == nationalIdHash, cancellationToken);

        CitizenFederatedProfile federatedProfile;
        bool isNewProfile;

        if (existingProfile != null)
        {
            // Profile exists - link to it
            federatedProfile = existingProfile;
            isNewProfile = false;

            // Check if this citizen is already linked to this profile
            var existingLink = federatedProfile.LinkedProfiles
                .FirstOrDefault(l => l.TenantId == citizen.TenantId && l.CitizenId == citizen.Id);

            if (existingLink != null)
            {
                if (existingLink.IsActive)
                {
                    return Error.Conflict(
                        "Link.AlreadyExists",
                        "Citizen is already linked to this federated profile");
                }
                else
                {
                    // Reactivate the link
                    existingLink.IsActive = true;
                    existingLink.LinkedAt = DateTime.UtcNow;
                    existingLink.UnlinkedAt = null;
                }
            }
            else
            {
                // Create new link
                var newLink = new LinkedCitizenProfile
                {
                    Id = Guid.NewGuid(),
                    FederatedProfileId = federatedProfile.Id,
                    TenantId = citizen.TenantId ?? Guid.Empty,
                    CitizenId = citizen.Id,
                    LinkedAt = DateTime.UtcNow,
                    IsActive = true
                };

                federatedProfile.LinkedProfiles.Add(newLink);
            }

            // Update consent if changed
            if (request.GrantConsent && federatedProfile.ConsentStatus != ConsentStatus.Granted)
            {
                federatedProfile.ConsentStatus = ConsentStatus.Granted;
                federatedProfile.ConsentChangedAt = DateTime.UtcNow;
            }

            federatedProfile.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            // Create new federated profile
            federatedProfile = new CitizenFederatedProfile
            {
                Id = Guid.NewGuid(),
                NationalIdType = (NationalIdType)request.NationalIdType,
                NationalIdEncrypted = encryptedNationalId,
                NationalIdHash = nationalIdHash,
                VerificationStatus = VerificationStatus.Pending,
                ConsentStatus = request.GrantConsent ? ConsentStatus.Granted : ConsentStatus.NotProvided,
                ConsentChangedAt = request.GrantConsent ? DateTime.UtcNow : null,
                CreatedAt = DateTime.UtcNow,
                LinkedProfiles = new List<LinkedCitizenProfile>
                {
                    new LinkedCitizenProfile
                    {
                        Id = Guid.NewGuid(),
                        TenantId = citizen.TenantId ?? Guid.Empty,
                        CitizenId = citizen.Id,
                        LinkedAt = DateTime.UtcNow,
                        IsActive = true
                    }
                }
            };

            _context.CitizenFederatedProfiles.Add(federatedProfile);
            isNewProfile = true;
        }

        // Update citizen record
        citizen.NationalIdType = (NationalIdType)request.NationalIdType;
        citizen.HasFederatedProfile = true;
        citizen.FederatedProfileId = federatedProfile.Id;
        citizen.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateOrLinkFederatedProfileResponse(
            federatedProfile.Id,
            federatedProfile.NationalIdType.ToString(),
            federatedProfile.VerificationStatus.ToString(),
            federatedProfile.ConsentStatus.ToString(),
            isNewProfile
                ? "Federated profile created successfully. Verification pending."
                : "Linked to existing federated profile successfully."
        );
    }
}
