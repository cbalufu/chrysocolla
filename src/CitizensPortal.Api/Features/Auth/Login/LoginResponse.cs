namespace CitizensPortal.Api.Features.Auth.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    CitizenInfo Citizen
);

public sealed record CitizenInfo(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    Guid? TenantId
);
