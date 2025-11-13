using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.MultiTenancy;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CrossCouncil.GetIssues;

public sealed class GetCrossCouncilIssuesQueryHandler
    : IRequestHandler<GetCrossCouncilIssuesQuery, ErrorOr<GetCrossCouncilIssuesResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantAccessor _tenantAccessor;

    public GetCrossCouncilIssuesQueryHandler(
        ApplicationDbContext context,
        ITenantAccessor tenantAccessor)
    {
        _context = context;
        _tenantAccessor = tenantAccessor;
    }

    public async Task<ErrorOr<GetCrossCouncilIssuesResponse>> Handle(
        GetCrossCouncilIssuesQuery request,
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
                "Citizen does not have a federated profile. Issues from current council only.");
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

        // Collect issues from all linked councils
        var allIssues = new List<AggregatedIssueDto>();

        foreach (var link in federatedProfile.LinkedProfiles.Where(l => l.IsActive))
        {
            // Get tenant name
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Id == link.TenantId, cancellationToken);

            var tenantName = tenant?.Name ?? "Unknown Council";

            // Query issues for this tenant-citizen combination
            var issuesQuery = _context.Issues
                .IgnoreQueryFilters() // Bypass tenant filter
                .Where(i => i.TenantId == link.TenantId && i.CitizenId == link.CitizenId);

            // Apply filters if provided
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                issuesQuery = issuesQuery.Where(i => i.Status == request.Status);
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                issuesQuery = issuesQuery.Where(i => i.Category == request.Category);
            }

            var issues = await issuesQuery
                .Select(i => new AggregatedIssueDto(
                    i.Id,
                    i.TenantId ?? Guid.Empty,
                    tenantName,
                    i.CitizenId,
                    i.Title,
                    i.Description,
                    i.Category,
                    i.Status,
                    i.Priority,
                    i.Location,
                    i.CreatedAt,
                    i.ResolvedAt
                ))
                .ToListAsync(cancellationToken);

            allIssues.AddRange(issues);
        }

        // Apply pagination
        var totalCount = allIssues.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var paginatedIssues = allIssues
            .OrderByDescending(i => i.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new GetCrossCouncilIssuesResponse(
            paginatedIssues,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );
    }
}
