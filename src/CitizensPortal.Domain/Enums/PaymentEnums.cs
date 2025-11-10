namespace CitizensPortal.Domain.Enums
{
    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        BankTransfer,
        EFT,
        Cash,
        Cheque,
        MobilePayment,
        OnlinePayment
    }

    public enum PaymentStatus
    {
        Pending,
        Processing,
        Completed,
        Failed,
        Cancelled,
        Refunded,
        PartiallyRefunded
    }

    public enum PaymentGateway
    {
        Stripe,
        PayPal,
        Square,
        Authorize,
        Paystack,
        Flutterwave,
        Internal
    }
}
