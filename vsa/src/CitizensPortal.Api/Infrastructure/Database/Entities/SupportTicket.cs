using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a support ticket for technical assistance
/// </summary>
public sealed class SupportTicket : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string TicketNumber { get; set; } = string.Empty; // Auto-generated
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Technical, Account, Payment, General

    public string Status { get; set; } = string.Empty; // Open, InProgress, Resolved, Closed
    public string Priority { get; set; } = string.Empty; // Low, Medium, High

    public Guid? AssignedToUserId { get; set; }
    public DateTime? AssignedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
}

/// <summary>
/// Messages in a support ticket conversation
/// </summary>
public sealed class TicketMessage : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid TicketId { get; set; }
    public SupportTicket Ticket { get; set; } = null!;

    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public bool IsStaff { get; set; }

    public string Message { get; set; } = string.Empty;
    public string? AttachmentUrls { get; set; } // JSON array of attachment URLs

    public DateTime CreatedAt { get; set; }
}
