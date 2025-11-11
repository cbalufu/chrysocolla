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

    public string Type { get; set; } = string.Empty; // Email, SMS, Push, InApp
    public string Category { get; set; } = string.Empty; // Alert, Reminder, Update, etc.
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    public string? ActionUrl { get; set; } // Deep link to relevant page
    public string? MetadataJson { get; set; } // Additional data as JSON

    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}
