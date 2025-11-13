using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.IdentityVerification.GetQueue;

public sealed record GetVerificationQueueQuery(
    VerificationRequestStatus? Status,
    int Page,
    int PageSize
) : IRequest<ErrorOr<GetVerificationQueueResponse>>;
