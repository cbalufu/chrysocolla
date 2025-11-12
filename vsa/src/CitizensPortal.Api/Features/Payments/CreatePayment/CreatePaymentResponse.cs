namespace CitizensPortal.Api.Features.Payments.CreatePayment;

public sealed record CreatePaymentResponse(
    Guid Id,
    string PaymentNumber,
    Guid BillId,
    string BillNumber,
    decimal Amount,
    string PaymentMethod,
    string Status,
    string? TransactionId,
    DateTime CreatedAt
);
