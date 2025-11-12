namespace CitizensPortal.Api.Features.Admin.CertificateRequests.UpdateRequestStatus;

public sealed record UpdateRequestStatusResponse(
    Guid Id,
    string ReferenceNumber,
    string Status,
    DateTime? ApprovalDate,
    DateTime? CollectionDate,
    string? RejectionReason,
    DateTime UpdatedAt
);
