namespace CitizensPortal.Api.Features.Appointments.BookAppointment;

public sealed record BookAppointmentResponse(
    Guid Id,
    string AppointmentType,
    string Department,
    string Purpose,
    DateTime ScheduledDate,
    TimeSpan ScheduledTime,
    int DurationMinutes,
    string Status,
    string? Notes,
    DateTime CreatedAt
);
