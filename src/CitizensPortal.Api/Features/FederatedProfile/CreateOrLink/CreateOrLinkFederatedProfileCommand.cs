using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.FederatedProfile.CreateOrLink;

public sealed record CreateOrLinkFederatedProfileCommand(
    Guid CitizenId,
    string NationalId,
    int NationalIdType,
    bool GrantConsent
) : IRequest<ErrorOr<CreateOrLinkFederatedProfileResponse>>;
