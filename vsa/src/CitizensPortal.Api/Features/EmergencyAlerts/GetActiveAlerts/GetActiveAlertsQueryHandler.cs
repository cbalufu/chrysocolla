using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.EmergencyAlerts.GetActiveAlerts;

public sealed class GetActiveAlertsQueryHandler
    : IRequestHandler<GetActiveAlertsQuery, ErrorOr<GetActiveAlertsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetActiveAlertsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetActiveAlertsResponse>> Handle(
        GetActiveAlertsQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var alerts = await _context.EmergencyAlerts
            .Where(a => a.IsActive && (a.ExpiresAt == null || a.ExpiresAt > now))
            .OrderByDescending(a => a.Severity == "Critical" ? 4 :
                                   a.Severity == "Severe" ? 3 :
                                   a.Severity == "Warning" ? 2 : 1)
            .ThenByDescending(a => a.CreatedAt)
            .Select(a => new EmergencyAlertDto(
                a.Id,
                a.AlertType,
                a.Title,
                a.Message,
                a.Severity,
                a.AffectedAreas,
                a.AcknowledgementCount,
                a.CreatedAt,
                a.ExpiresAt
            ))
            .ToListAsync(cancellationToken);

        return new GetActiveAlertsResponse(alerts);
    }
}
