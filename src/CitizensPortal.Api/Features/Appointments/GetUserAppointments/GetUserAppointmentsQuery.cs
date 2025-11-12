using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Appointments.GetUserAppointments;

public sealed record GetUserAppointmentsQuery(
    Guid CitizenId,
    string? Status = null
) : IRequest<ErrorOr<GetUserAppointmentsResponse>>;
