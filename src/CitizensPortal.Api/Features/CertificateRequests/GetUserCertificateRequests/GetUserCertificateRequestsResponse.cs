namespace CitizensPortal.Api.Features.CertificateRequests.GetUserCertificateRequests;

public sealed record GetUserCertificateRequestsResponse(
    List<CertificateRequestDto> Requests
);

public sealed record CertificateRequestDto(
    Guid Id,
    string ReferenceNumber,
    string CertificateType,
    string Status,
    string DeliveryMethod,
    decimal ApplicationFee,
    bool IsPaid,
    DateTime CreatedAt,
    DateTime? ApprovalDate,
    DateTime? CollectionDate
);
