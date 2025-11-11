namespace CitizensPortal.Api.Features.Citizens.Register;

public sealed record RegisterCitizenResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? NationalId,
    DateTime DateOfBirth,
    string Address,
    DateTime CreatedAt
);
