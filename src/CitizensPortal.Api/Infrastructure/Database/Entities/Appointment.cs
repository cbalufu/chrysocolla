using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents an appointment booked by a citizen with municipal services
/// </summary>
public sealed class Appointment : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string AppointmentType { get; set; } = string.Empty; // Consultation, DocumentSubmission, etc.
    public string Department { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;

    public DateTime ScheduledDate { get; set; }
    public TimeSpan ScheduledTime { get; set; }
    public int DurationMinutes { get; set; }

    public string Status { get; set; } = string.Empty; // Scheduled, Confirmed, Completed, Cancelled, NoShow
    public string? Location { get; set; }
    public string? RoomNumber { get; set; }

    public Guid? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }

    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}
