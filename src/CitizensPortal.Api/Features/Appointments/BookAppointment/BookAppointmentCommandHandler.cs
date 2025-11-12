using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Appointments.BookAppointment;

public sealed class BookAppointmentCommandHandler
    : IRequestHandler<BookAppointmentCommand, ErrorOr<BookAppointmentResponse>>
{
    private readonly ApplicationDbContext _context;

    public BookAppointmentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<BookAppointmentResponse>> Handle(
        BookAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        // Verify citizen exists
        var citizenExists = await _context.Citizens
            .AnyAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (!citizenExists)
        {
            return Error.NotFound(
                code: "Citizen.NotFound",
                description: "Citizen not found");
        }

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            CitizenId = request.CitizenId,
            AppointmentType = request.AppointmentType,
            Department = request.Department,
            Purpose = request.Purpose,
            ScheduledDate = request.ScheduledDate,
            ScheduledTime = request.ScheduledTime,
            DurationMinutes = request.DurationMinutes,
            Status = "Scheduled",
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        return new BookAppointmentResponse(
            appointment.Id,
            appointment.AppointmentType,
            appointment.Department,
            appointment.Purpose,
            appointment.ScheduledDate,
            appointment.ScheduledTime,
            appointment.DurationMinutes,
            appointment.Status,
            appointment.Notes,
            appointment.CreatedAt
        );
    }
}
