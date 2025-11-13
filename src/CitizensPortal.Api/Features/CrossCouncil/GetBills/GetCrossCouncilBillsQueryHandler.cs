using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.MultiTenancy;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.CrossCouncil.GetBills;

public sealed class GetCrossCouncilBillsQueryHandler
    : IRequestHandler<GetCrossCouncilBillsQuery, ErrorOr<GetCrossCouncilBillsResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantAccessor _tenantAccessor;

    public GetCrossCouncilBillsQueryHandler(
        ApplicationDbContext context,
        ITenantAccessor tenantAccessor)
    {
        _context = context;
        _tenantAccessor = tenantAccessor;
    }

    public async Task<ErrorOr<GetCrossCouncilBillsResponse>> Handle(
        GetCrossCouncilBillsQuery request,
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
                "Citizen does not have a federated profile. Bills from current council only.");
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

        // Collect bills from all linked councils
        var allBills = new List<AggregatedBillDto>();
        decimal totalOutstanding = 0;

        foreach (var link in federatedProfile.LinkedProfiles.Where(l => l.IsActive))
        {
            // Get tenant name
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Id == link.TenantId, cancellationToken);

            var tenantName = tenant?.Name ?? "Unknown Council";

            // Query bills for this tenant-citizen combination
            var billsQuery = _context.Bills
                .IgnoreQueryFilters() // Bypass tenant filter
                .Where(b => b.TenantId == link.TenantId && b.CitizenId == link.CitizenId);

            // Apply status filter if provided
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                billsQuery = billsQuery.Where(b => b.Status == request.Status);
            }

            var bills = await billsQuery
                .Select(b => new AggregatedBillDto(
                    b.Id,
                    b.TenantId ?? Guid.Empty,
                    tenantName,
                    b.CitizenId,
                    b.BillNumber,
                    b.BillType,
                    b.Description,
                    b.Amount,
                    b.TotalAmount,
                    b.Status,
                    b.DueDate,
                    b.IssuedAt,
                    b.PaidAt
                ))
                .ToListAsync(cancellationToken);

            // Calculate outstanding amount (Unpaid or Overdue)
            var outstanding = bills
                .Where(b => b.Status == "Unpaid" || b.Status == "Overdue")
                .Sum(b => b.TotalAmount);

            totalOutstanding += outstanding;
            allBills.AddRange(bills);
        }

        // Apply pagination
        var totalCount = allBills.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var paginatedBills = allBills
            .OrderByDescending(b => b.IssuedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new GetCrossCouncilBillsResponse(
            paginatedBills,
            totalOutstanding,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );
    }
}
