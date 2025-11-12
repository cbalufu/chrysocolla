using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Appointments.GetUserAppointments;

public sealed class GetUserAppointmentsQueryHandler
    : IRequestHandler<GetUserAppointmentsQuery, ErrorOr<GetUserAppointmentsResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetUserAppointmentsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetUserAppointmentsResponse>> Handle(
        GetUserAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Appointments
            .Where(a => a.CitizenId == request.CitizenId);

        // Filter by status if provided
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(a => a.Status == request.Status);
        }

        var appointments = await query
            .OrderByDescending(a => a.ScheduledDate)
            .ThenByDescending(a => a.ScheduledTime)
            .Select(a => new AppointmentDto(
                a.Id,
                a.AppointmentType,
                a.Department,
                a.Purpose,
                a.ScheduledDate,
                a.ScheduledTime,
                a.DurationMinutes,
                a.Status,
                a.Location,
                a.Notes,
                a.CreatedAt,
                a.UpdatedAt,
                a.CancelledAt
            ))
            .ToListAsync(cancellationToken);

        return new GetUserAppointmentsResponse(appointments);
    }
}
