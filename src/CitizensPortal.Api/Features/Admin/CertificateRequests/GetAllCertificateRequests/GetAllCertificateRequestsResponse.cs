namespace CitizensPortal.Api.Features.Admin.CertificateRequests.GetAllCertificateRequests;

public sealed record GetAllCertificateRequestsResponse(
    List<AdminCertificateRequestDto> Requests,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public sealed record AdminCertificateRequestDto(
    Guid Id,
    string ReferenceNumber,
    Guid CitizenId,
    string CitizenName,
    string CitizenEmail,
    string CertificateType,
    string Status,
    string Purpose,
    string DeliveryMethod,
    decimal ApplicationFee,
    bool IsPaid,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ApprovalDate,
    DateTime? CollectionDate,
    string? RejectionReason
);
