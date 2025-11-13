using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.FederatedProfile.ManageConsent;

public sealed record UpdateConsentCommand(
    Guid CitizenId,
    bool GrantConsent
) : IRequest<ErrorOr<UpdateConsentResponse>>;
