namespace CitizensPortal.Api.Features.EmergencyAlerts.CreateAlert;

public sealed record CreateAlertResponse(
    Guid Id,
    string AlertType,
    string Title,
    string Message,
    string Severity,
    string? AffectedAreas,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? ExpiresAt
);
