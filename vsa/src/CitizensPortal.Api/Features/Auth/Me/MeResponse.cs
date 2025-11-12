namespace CitizensPortal.Api.Features.Auth.Me;

public sealed record MeResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? NationalId,
    DateTime DateOfBirth,
    string Address,
    Guid? TenantId,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    bool IsActive
);
