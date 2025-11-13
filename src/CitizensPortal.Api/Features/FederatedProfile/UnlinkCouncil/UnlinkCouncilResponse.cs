namespace CitizensPortal.Api.Features.FederatedProfile.UnlinkCouncil;

public sealed record UnlinkCouncilResponse(
    Guid TenantId,
    string TenantName,
    DateTime UnlinkedAt,
    int RemainingLinkedCouncils,
    string Message
);
