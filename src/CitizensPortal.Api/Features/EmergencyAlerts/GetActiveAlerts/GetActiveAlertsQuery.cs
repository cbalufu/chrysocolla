using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.EmergencyAlerts.GetActiveAlerts;

public sealed record GetActiveAlertsQuery() : IRequest<ErrorOr<GetActiveAlertsResponse>>;
