using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.FederatedProfile.UnlinkCouncil;

public sealed class UnlinkCouncilCommandHandler
    : IRequestHandler<UnlinkCouncilCommand, ErrorOr<UnlinkCouncilResponse>>
{
    private readonly ApplicationDbContext _context;

    public UnlinkCouncilCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UnlinkCouncilResponse>> Handle(
        UnlinkCouncilCommand request,
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

        // Find the link to unlink
        var linkToUnlink = federatedProfile.LinkedProfiles
            .FirstOrDefault(l => l.TenantId == request.TenantId && l.IsActive);

        if (linkToUnlink == null)
        {
            return Error.NotFound(
                "Link.NotFound",
                "No active link found to this council");
        }

        // Get tenant name
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        var tenantName = tenant?.Name ?? "Unknown Council";

        // Check if this is the citizen's current tenant
        if (citizen.TenantId == request.TenantId)
        {
            return Error.Validation(
                "Unlink.CurrentTenant",
                "Cannot unlink from your current council. Please switch to another council first.");
        }

        // Unlink (soft delete - keep for audit)
        linkToUnlink.IsActive = false;
        linkToUnlink.UnlinkedAt = DateTime.UtcNow;
        federatedProfile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // Count remaining active links
        var remainingLinks = federatedProfile.LinkedProfiles.Count(l => l.IsActive);

        return new UnlinkCouncilResponse(
            request.TenantId,
            tenantName,
            linkToUnlink.UnlinkedAt.Value,
            remainingLinks,
            $"Successfully unlinked from {tenantName}. Data from this council will no longer appear in cross-council views."
        );
    }
}
