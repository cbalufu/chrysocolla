using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.EmergencyAlerts.AcknowledgeAlert;

public sealed class AcknowledgeAlertCommandHandler
    : IRequestHandler<AcknowledgeAlertCommand, ErrorOr<AcknowledgeAlertResponse>>
{
    private readonly ApplicationDbContext _context;

    public AcknowledgeAlertCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<AcknowledgeAlertResponse>> Handle(
        AcknowledgeAlertCommand request,
        CancellationToken cancellationToken)
    {
        // Verify alert exists
        var alert = await _context.EmergencyAlerts
            .FirstOrDefaultAsync(a => a.Id == request.AlertId, cancellationToken);

        if (alert == null)
        {
            return Error.NotFound(
                code: "EmergencyAlert.NotFound",
                description: "Emergency alert not found");
        }

        // Check if citizen has already acknowledged this alert
        var existingAcknowledgement = await _context.AlertAcknowledgements
            .FirstOrDefaultAsync(a => a.AlertId == request.AlertId && a.CitizenId == request.CitizenId, cancellationToken);

        if (existingAcknowledgement != null)
        {
            return Error.Conflict(
                code: "AlertAcknowledgement.AlreadyExists",
                description: "You have already acknowledged this alert");
        }

        // Create acknowledgement
        var acknowledgement = new AlertAcknowledgement
        {
            Id = Guid.NewGuid(),
            AlertId = request.AlertId,
            CitizenId = request.CitizenId,
            AcknowledgedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.AlertAcknowledgements.Add(acknowledgement);

        // Increment acknowledgement count on the alert
        alert.AcknowledgementCount++;

        await _context.SaveChangesAsync(cancellationToken);

        return new AcknowledgeAlertResponse(
            acknowledgement.Id,
            acknowledgement.AlertId,
            acknowledgement.CitizenId,
            acknowledgement.AcknowledgedAt
        );
    }
}
