using Carter;
using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace CitizensPortal.Api.Features.Appointments.BookAppointment;

public sealed class BookAppointment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointments", async (
            BookAppointmentRequest request,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            // Get user ID from JWT claims
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new BookAppointmentCommand(
                userId,
                request.AppointmentType,
                request.Department,
                request.Purpose,
                request.ScheduledDate,
                request.ScheduledTime,
                request.DurationMinutes,
                request.Notes
            );

            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                success => Results.Ok(success),
                errors => Results.Problem(statusCode: errors[0].Type switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status400BadRequest
                },
                title: errors[0].Code,
                detail: errors[0].Description)
            );
        })
        .RequireAuthorization()
        .WithName("BookAppointment")
        .WithTags("Appointments")
        .WithOpenApi()
        .Produces<BookAppointmentResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Book an appointment")
        .WithDescription("Allows citizens to book appointments with municipal departments.");
    }
}

public sealed record BookAppointmentRequest(
    string AppointmentType,
    string Department,
    string Purpose,
    DateTime ScheduledDate,
    TimeSpan ScheduledTime,
    int DurationMinutes,
    string? Notes
);
