using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.MultiTenancy;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CrossCouncil.GetSummary;

public sealed class GetCrossCouncilSummaryQueryHandler
    : IRequestHandler<GetCrossCouncilSummaryQuery, ErrorOr<GetCrossCouncilSummaryResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantAccessor _tenantAccessor;

    public GetCrossCouncilSummaryQueryHandler(
        ApplicationDbContext context,
        ITenantAccessor tenantAccessor)
    {
        _context = context;
        _tenantAccessor = tenantAccessor;
    }

    public async Task<ErrorOr<GetCrossCouncilSummaryResponse>> Handle(
        GetCrossCouncilSummaryQuery request,
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
                "Citizen does not have a federated profile");
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

        // Collect summary data from all linked councils
        var councilSummaries = new List<CouncilSummaryDto>();
        int totalProperties = 0;
        int totalBills = 0;
        decimal totalOutstanding = 0;
        int totalIssues = 0;
        int totalOpenIssues = 0;

        foreach (var link in federatedProfile.LinkedProfiles.Where(l => l.IsActive))
        {
            // Get tenant name
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Id == link.TenantId, cancellationToken);

            var tenantName = tenant?.Name ?? "Unknown Council";

            // Count properties
            var propertiesCount = await _context.Properties
                .IgnoreQueryFilters()
                .Where(p => p.TenantId == link.TenantId && p.OwnerId == link.CitizenId)
                .CountAsync(cancellationToken);

            // Count bills and calculate outstanding
            var bills = await _context.Bills
                .IgnoreQueryFilters()
                .Where(b => b.TenantId == link.TenantId && b.CitizenId == link.CitizenId)
                .ToListAsync(cancellationToken);

            var billsCount = bills.Count;
            var outstanding = bills
                .Where(b => b.Status == "Unpaid" || b.Status == "Overdue")
                .Sum(b => b.TotalAmount);

            // Count issues
            var issues = await _context.Issues
                .IgnoreQueryFilters()
                .Where(i => i.TenantId == link.TenantId && i.CitizenId == link.CitizenId)
                .ToListAsync(cancellationToken);

            var issuesCount = issues.Count;
            var openIssuesCount = issues
                .Count(i => i.Status == "Reported" || i.Status == "InProgress");

            // Add to council summaries
            councilSummaries.Add(new CouncilSummaryDto(
                link.TenantId,
                tenantName,
                propertiesCount,
                billsCount,
                outstanding,
                issuesCount,
                openIssuesCount
            ));

            // Add to totals
            totalProperties += propertiesCount;
            totalBills += billsCount;
            totalOutstanding += outstanding;
            totalIssues += issuesCount;
            totalOpenIssues += openIssuesCount;
        }

        var totals = new CrossCouncilTotalsDto(
            totalProperties,
            totalBills,
            totalOutstanding,
            totalIssues,
            totalOpenIssues
        );

        return new GetCrossCouncilSummaryResponse(
            councilSummaries.Count,
            councilSummaries,
            totals
        );
    }
}
