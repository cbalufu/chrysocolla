using FluentValidation;

namespace CitizensPortal.Api.Features.Payments.CreatePayment;

public sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required")
            .MaximumLength(50).WithMessage("Payment method must not exceed 50 characters")
            .Must(BeValidPaymentMethod).WithMessage("Invalid payment method. Valid methods: CreditCard, DebitCard, BankTransfer, Cash, Check, MobileMoney");

        RuleFor(x => x.TransactionId)
            .MaximumLength(100).WithMessage("Transaction ID must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.TransactionId));
    }

    private bool BeValidPaymentMethod(string paymentMethod)
    {
        var validMethods = new[] { "CreditCard", "DebitCard", "BankTransfer", "Cash", "Check", "MobileMoney" };
        return validMethods.Contains(paymentMethod);
    }
}
