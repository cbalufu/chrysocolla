using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Appointments.BookAppointment;

public sealed record BookAppointmentCommand(
    Guid CitizenId,
    string AppointmentType,
    string Department,
    string Purpose,
    DateTime ScheduledDate,
    TimeSpan ScheduledTime,
    int DurationMinutes,
    string? Notes
) : IRequest<ErrorOr<BookAppointmentResponse>>;
