namespace CitizensPortal.Api.Features.CertificateRequests.GetCertificateRequestById;

public sealed record GetCertificateRequestByIdResponse(
    Guid Id,
    string ReferenceNumber,
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
