using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CertificateRequests.GetCertificateRequestById;

public sealed record GetCertificateRequestByIdQuery(
    Guid RequestId,
    Guid CitizenId
) : IRequest<ErrorOr<GetCertificateRequestByIdResponse>>;
