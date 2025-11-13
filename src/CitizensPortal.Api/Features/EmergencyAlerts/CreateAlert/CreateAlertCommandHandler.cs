using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.EmergencyAlerts.CreateAlert;

public sealed class CreateAlertCommandHandler
    : IRequestHandler<CreateAlertCommand, ErrorOr<CreateAlertResponse>>
{
    private readonly ApplicationDbContext _context;

    public CreateAlertCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<CreateAlertResponse>> Handle(
        CreateAlertCommand request,
        CancellationToken cancellationToken)
    {
        var alert = new EmergencyAlert
        {
            Id = Guid.NewGuid(),
            AlertType = request.AlertType,
            Title = request.Title,
            Message = request.Message,
            Severity = request.Severity,
            AffectedAreas = request.AffectedAreas,
            IsActive = true,
            AcknowledgementCount = 0,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = request.ExpiresAt
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.EmergencyAlerts.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateAlertResponse(
            alert.Id,
            alert.AlertType,
            alert.Title,
            alert.Message,
            alert.Severity,
            alert.AffectedAreas,
            alert.IsActive,
            alert.CreatedAt,
            alert.ExpiresAt
        );
    }
}
