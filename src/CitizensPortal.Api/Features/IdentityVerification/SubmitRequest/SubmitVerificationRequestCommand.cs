using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.IdentityVerification.SubmitRequest;

public sealed record SubmitVerificationRequestCommand(
    Guid CitizenId,
    NationalIdType NationalIdType,
    string NationalIdValue
) : IRequest<ErrorOr<SubmitVerificationRequestResponse>>;
