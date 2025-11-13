namespace CitizensPortal.Api.Features.EmergencyAlerts.GetActiveAlerts;

public sealed record GetActiveAlertsResponse(
    List<EmergencyAlertDto> Alerts
);

public sealed record EmergencyAlertDto(
    Guid Id,
    string AlertType,
    string Title,
    string Message,
    string Severity,
    string? AffectedAreas,
    int AcknowledgementCount,
    DateTime CreatedAt,
    DateTime? ExpiresAt
);
