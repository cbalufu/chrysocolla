using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilBranding.GetBranding;

public sealed record GetCouncilBrandingQuery(
    Guid? TenantId = null // If null, gets current tenant's branding
) : IRequest<ErrorOr<GetCouncilBrandingResponse>>;
