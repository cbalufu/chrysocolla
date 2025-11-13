using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.MultiTenancy;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CrossCouncil.GetProperties;

public sealed class GetCrossCouncilPropertiesQueryHandler
    : IRequestHandler<GetCrossCouncilPropertiesQuery, ErrorOr<GetCrossCouncilPropertiesResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantAccessor _tenantAccessor;

    public GetCrossCouncilPropertiesQueryHandler(
        ApplicationDbContext context,
        ITenantAccessor tenantAccessor)
    {
        _context = context;
        _tenantAccessor = tenantAccessor;
    }

    public async Task<ErrorOr<GetCrossCouncilPropertiesResponse>> Handle(
        GetCrossCouncilPropertiesQuery request,
        CancellationToken cancellationToken)
    {
        // Get citizen's federated profile
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
                "Citizen does not have a federated profile. Properties from current council only.");
        }

        // Get federated profile with linked councils
        var federatedProfile = await _context.CitizenFederatedProfiles
            .Include(p => p.LinkedProfiles.Where(l => l.IsActive))
            .FirstOrDefaultAsync(p => p.Id == citizen.FederatedProfileId, cancellationToken);

        if (federatedProfile == null)
        {
            return Error.NotFound(
                "FederatedProfile.NotFound",
                "Federated profile not found");
        }

        // Check consent
        if (federatedProfile.ConsentStatus != Infrastructure.Database.Entities.ConsentStatus.Granted)
        {
            return Error.Forbidden(
                "Consent.NotGranted",
                "Cross-council data sharing consent has not been granted");
        }

        // Collect properties from all linked councils
        var allProperties = new List<AggregatedPropertyDto>();

        foreach (var link in federatedProfile.LinkedProfiles.Where(l => l.IsActive))
        {
            // Get tenant name
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Id == link.TenantId, cancellationToken);

            var tenantName = tenant?.Name ?? "Unknown Council";

            // Query properties for this tenant-citizen combination
            // We need to temporarily bypass the tenant filter
            var properties = await _context.Properties
                .IgnoreQueryFilters() // Bypass tenant filter
                .Where(p => p.TenantId == link.TenantId && p.OwnerId == link.CitizenId)
                .Select(p => new AggregatedPropertyDto(
                    p.Id,
                    p.TenantId ?? Guid.Empty,
                    tenantName,
                    p.OwnerId,
                    p.PropertyNumber,
                    p.PropertyType,
                    p.Address,
                    p.AssessedValue,
                    p.AnnualTaxAmount,
                    p.Status,
                    p.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            allProperties.AddRange(properties);
        }

        // Apply pagination
        var totalCount = allProperties.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var paginatedProperties = allProperties
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new GetCrossCouncilPropertiesResponse(
            paginatedProperties,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );
    }
}
