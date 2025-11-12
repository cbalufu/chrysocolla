using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CertificateRequests.SubmitCertificateRequest;

public sealed record SubmitCertificateRequestCommand(
    Guid CitizenId,
    string CertificateType,
    string Purpose,
    string DeliveryMethod
) : IRequest<ErrorOr<SubmitCertificateRequestResponse>>;
