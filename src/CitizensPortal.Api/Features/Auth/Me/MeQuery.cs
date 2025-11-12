using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Auth.Me;

public sealed record MeQuery(Guid UserId) : IRequest<ErrorOr<MeResponse>>;
