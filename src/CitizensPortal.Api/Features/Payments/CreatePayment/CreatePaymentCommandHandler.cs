using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Payments.CreatePayment;

public sealed class CreatePaymentCommandHandler
    : IRequestHandler<CreatePaymentCommand, ErrorOr<CreatePaymentResponse>>
{
    private readonly ApplicationDbContext _context;

    public CreatePaymentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<CreatePaymentResponse>> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        // Verify bill exists and belongs to citizen
        var bill = await _context.Bills
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.Id == request.BillId, cancellationToken);

        if (bill == null)
        {
            return Error.NotFound(
                code: "Bill.NotFound",
                description: "Bill not found");
        }

        if (bill.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "Bill.AccessDenied",
                description: "You do not have permission to pay this bill");
        }

        if (bill.Status == "Paid")
        {
            return Error.Validation(
                code: "Bill.AlreadyPaid",
                description: "This bill has already been paid");
        }

        // Calculate total paid amount including this payment
        var totalPaid = bill.Payments.Sum(p => p.Amount) + request.Amount;

        if (totalPaid > bill.TotalAmount)
        {
            return Error.Validation(
                code: "Payment.ExceedsTotal",
                description: $"Payment amount exceeds bill total. Remaining: {bill.TotalAmount - bill.Payments.Sum(p => p.Amount)}");
        }

        // Generate payment number
        var paymentNumber = $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            PaymentNumber = paymentNumber,
            BillId = request.BillId,
            CitizenId = request.CitizenId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            Status = "Completed",
            TransactionId = request.TransactionId,
            CreatedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.Payments.Add(payment);

        // Update bill status if fully paid
        if (totalPaid >= bill.TotalAmount)
        {
            bill.Status = "Paid";
            bill.PaidAt = DateTime.UtcNow;
        }
        else
        {
            bill.Status = "PartiallyPaid";
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new CreatePaymentResponse(
            payment.Id,
            payment.PaymentNumber,
            payment.BillId!.Value,
            bill.BillNumber,
            payment.Amount,
            payment.PaymentMethod,
            payment.Status,
            payment.TransactionId,
            payment.CreatedAt
        );
    }
}
