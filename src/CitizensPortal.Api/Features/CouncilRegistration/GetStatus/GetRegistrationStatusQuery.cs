using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetStatus;

public sealed record GetRegistrationStatusQuery(
    string ReferenceNumber
) : IRequest<ErrorOr<GetRegistrationStatusResponse>>;
