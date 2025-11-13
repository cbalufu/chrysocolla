namespace CitizensPortal.Api.Features.FederatedProfile.ManageConsent;

public sealed record UpdateConsentResponse(
    Guid FederatedProfileId,
    string ConsentStatus,
    DateTime ConsentChangedAt,
    string Message
);
