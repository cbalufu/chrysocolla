using CitizensPortal.Api.Infrastructure.MultiTenancy;

namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Represents a payment made by a citizen
/// </summary>
public sealed class Payment : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public Guid CitizenId { get; set; }
    public Citizen Citizen { get; set; } = null!;

    public Guid? BillId { get; set; }
    public Bill? Bill { get; set; }

    public string PaymentNumber { get; set; } = string.Empty; // Auto-generated unique number
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty; // CreditCard, DebitCard, BankTransfer, Cash, MobileMoney

    public string Status { get; set; } = string.Empty; // Pending, Processing, Completed, Failed, Refunded
    public string? TransactionId { get; set; } // External payment gateway transaction ID
    public string? ReceiptNumber { get; set; }

    public string? PaymentGateway { get; set; } // Stripe, PayPal, etc.
    public string? PaymentGatewayResponse { get; set; } // JSON response from gateway

    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
