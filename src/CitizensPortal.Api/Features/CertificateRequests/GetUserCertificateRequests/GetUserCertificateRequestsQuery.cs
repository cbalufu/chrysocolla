using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CertificateRequests.GetUserCertificateRequests;

public sealed record GetUserCertificateRequestsQuery(
    Guid CitizenId,
    string? Status,
    string? CertificateType
) : IRequest<ErrorOr<GetUserCertificateRequestsResponse>>;
