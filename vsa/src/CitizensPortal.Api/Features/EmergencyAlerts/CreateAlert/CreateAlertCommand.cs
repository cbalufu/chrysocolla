using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.EmergencyAlerts.CreateAlert;

public sealed record CreateAlertCommand(
    string AlertType,
    string Title,
    string Message,
    string Severity,
    string? AffectedAreas,
    DateTime? ExpiresAt
) : IRequest<ErrorOr<CreateAlertResponse>>;
