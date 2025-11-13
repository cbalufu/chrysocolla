using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilRegistration.SubmitRegistration;

public sealed record SubmitCouncilRegistrationCommand(
    string CouncilName,
    string RegistrationNumber,
    string Region,
    string ContactName,
    string ContactEmail,
    string ContactPhone,
    string AdminName,
    string AdminEmail,
    string PhysicalAddress,
    string City,
    string PostalCode
) : IRequest<ErrorOr<SubmitCouncilRegistrationResponse>>;
