namespace CitizensPortal.Api.Features.Auth.Refresh;

public sealed record RefreshResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
