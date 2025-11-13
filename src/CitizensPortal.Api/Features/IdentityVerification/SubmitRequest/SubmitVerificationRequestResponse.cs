namespace CitizensPortal.Api.Features.IdentityVerification.SubmitRequest;

public sealed record SubmitVerificationRequestResponse(
    Guid RequestId,
    string ReferenceNumber,
    DateTime SubmittedAt,
    DateTime ExpiresAt,
    string Message
);
