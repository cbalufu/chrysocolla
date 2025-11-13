namespace CitizensPortal.Api.Features.CouncilBranding.UpdateBranding;

public sealed record UpdateCouncilBrandingResponse(
    Guid TenantId,
    string TenantName,
    string Message
);
