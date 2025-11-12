namespace CitizensPortal.Api.Features.CertificateRequests.SubmitCertificateRequest;

public sealed record SubmitCertificateRequestResponse(
    Guid Id,
    string ReferenceNumber,
    string CertificateType,
    string Purpose,
    string DeliveryMethod,
    string Status,
    decimal ApplicationFee,
    bool IsPaid,
    DateTime CreatedAt
);
