using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a notification sent to a citizen
/// </summary>
public sealed class Notification : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string Type { get; set; } = string.Empty; // Bill, Application, Issue, Ticket, Announcement, EmergencyAlert
    public string Priority { get; set; } = string.Empty; // Low, Medium, High, Urgent
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;
    public DateTime SentDate { get; set; }
    public DateTime? ReadDate { get; set; }
}
