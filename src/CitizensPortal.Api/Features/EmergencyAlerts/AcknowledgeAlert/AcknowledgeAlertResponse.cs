namespace CitizensPortal.Api.Features.EmergencyAlerts.AcknowledgeAlert;

public sealed record AcknowledgeAlertResponse(
    Guid Id,
    Guid AlertId,
    Guid CitizenId,
    DateTime AcknowledgedAt
);
