using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a bill issued to a citizen for municipal services
/// </summary>
public sealed class Bill : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public string BillNumber { get; set; } = string.Empty; // Auto-generated unique number
    public string BillType { get; set; } = string.Empty; // PropertyTax, WaterBill, WasteFee, etc.
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = string.Empty; // Unpaid, PartiallyPaid, Paid, Overdue
    public DateTime DueDate { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime? PaidAt { get; set; }

    public string? PropertyReference { get; set; }
    public string? AccountNumber { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
