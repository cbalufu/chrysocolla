namespace CitizensPortal.Api.Infrastructure.Authentication;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email, Guid? tenantId, string role);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiryTime();
}
