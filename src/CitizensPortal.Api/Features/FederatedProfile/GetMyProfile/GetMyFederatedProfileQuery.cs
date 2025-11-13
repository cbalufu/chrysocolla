using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.FederatedProfile.GetMyProfile;

public sealed record GetMyFederatedProfileQuery(
    Guid CitizenId
) : IRequest<ErrorOr<GetMyFederatedProfileResponse>>;
