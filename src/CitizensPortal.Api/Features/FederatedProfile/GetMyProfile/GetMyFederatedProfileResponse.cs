namespace CitizensPortal.Api.Features.FederatedProfile.GetMyProfile;

public sealed record GetMyFederatedProfileResponse(
    Guid FederatedProfileId,
    string NationalIdType,
    string VerificationStatus,
    string? VerificationMethod,
    DateTime? VerifiedAt,
    string ConsentStatus,
    DateTime? ConsentChangedAt,
    List<LinkedCouncilDto> LinkedCouncils
);

public sealed record LinkedCouncilDto(
    Guid TenantId,
    Guid CitizenId,
    DateTime LinkedAt,
    bool IsActive
);
