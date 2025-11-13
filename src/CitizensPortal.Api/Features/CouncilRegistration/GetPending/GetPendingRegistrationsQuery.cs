using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetPending;

public sealed record GetPendingRegistrationsQuery(
    CouncilRegistrationStatus? Status,
    int Page,
    int PageSize
) : IRequest<ErrorOr<GetPendingRegistrationsResponse>>;
