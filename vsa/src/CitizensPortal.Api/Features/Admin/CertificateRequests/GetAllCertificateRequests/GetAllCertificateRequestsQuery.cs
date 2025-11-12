using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.CertificateRequests.GetAllCertificateRequests;

public sealed record GetAllCertificateRequestsQuery(
    int Page,
    int PageSize,
    string? Status,
    string? CertificateType,
    Guid? CitizenId
) : IRequest<ErrorOr<GetAllCertificateRequestsResponse>>;
