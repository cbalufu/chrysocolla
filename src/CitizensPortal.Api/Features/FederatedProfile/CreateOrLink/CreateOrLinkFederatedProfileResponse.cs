namespace CitizensPortal.Api.Features.FederatedProfile.CreateOrLink;

public sealed record CreateOrLinkFederatedProfileResponse(
    Guid FederatedProfileId,
    string NationalIdType,
    string VerificationStatus,
    string ConsentStatus,
    string Message
);
