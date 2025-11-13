using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.FederatedProfile.UnlinkCouncil;

public sealed record UnlinkCouncilCommand(
    Guid CitizenId,
    Guid TenantId
) : IRequest<ErrorOr<UnlinkCouncilResponse>>;
