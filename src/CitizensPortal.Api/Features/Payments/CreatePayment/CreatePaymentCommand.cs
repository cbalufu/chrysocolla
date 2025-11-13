using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Payments.CreatePayment;

public sealed record CreatePaymentCommand(
    Guid CitizenId,
    Guid BillId,
    decimal Amount,
    string PaymentMethod,
    string? TransactionId
) : IRequest<ErrorOr<CreatePaymentResponse>>;
