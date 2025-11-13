namespace CitizensPortal.Api.Features.CouncilRegistration.SubmitRegistration;

public sealed record SubmitCouncilRegistrationResponse(
    Guid RequestId,
    string ReferenceNumber,
    DateTime SubmittedAt,
    DateTime ExpiresAt,
    string Message
);
