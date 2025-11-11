using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Citizens.Register;

public sealed record RegisterCitizenCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? NationalId,
    DateTime DateOfBirth,
    string Address
) : IRequest<ErrorOr<RegisterCitizenResponse>>;
