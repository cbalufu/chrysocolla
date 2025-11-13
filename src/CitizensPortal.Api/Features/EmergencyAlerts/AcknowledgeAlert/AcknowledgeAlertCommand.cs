using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.EmergencyAlerts.AcknowledgeAlert;

public sealed record AcknowledgeAlertCommand(
    Guid AlertId,
    Guid CitizenId
) : IRequest<ErrorOr<AcknowledgeAlertResponse>>;
