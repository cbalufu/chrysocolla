namespace CitizensPortal.Api.Features.Appointments.GetUserAppointments;

public sealed record GetUserAppointmentsResponse(
    List<AppointmentDto> Appointments
);

public sealed record AppointmentDto(
    Guid Id,
    string AppointmentType,
    string Department,
    string Purpose,
    DateTime ScheduledDate,
    TimeSpan ScheduledTime,
    int DurationMinutes,
    string Status,
    string? Location,
    string? Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? CancelledAt
);
