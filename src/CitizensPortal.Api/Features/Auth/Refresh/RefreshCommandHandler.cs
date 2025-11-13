using CitizensPortal.Api.Infrastructure.Authentication;
using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Auth.Refresh;

public sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, ErrorOr<RefreshResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshCommandHandler(ApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ErrorOr<RefreshResponse>> Handle(
        RefreshCommand request,
        CancellationToken cancellationToken)
    {
        // Find citizen by refresh token (tenant context is automatically applied)
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.RefreshToken == request.RefreshToken, cancellationToken);

        if (citizen == null)
        {
            return Error.Unauthorized(
                code: "Auth.InvalidRefreshToken",
                description: "Invalid refresh token");
        }

        // Check if refresh token has expired
        if (citizen.RefreshTokenExpiryTime == null || citizen.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return Error.Unauthorized(
                code: "Auth.RefreshTokenExpired",
                description: "Refresh token has expired");
        }

        // Check if citizen is active
        if (!citizen.IsActive)
        {
            return Error.Forbidden(
                code: "Auth.AccountInactive",
                description: "Your account has been deactivated");
        }

        // Generate new tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(
            citizen.Id,
            citizen.Email,
            citizen.TenantId,
            citizen.Role);

        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiry = _jwtTokenService.GetRefreshTokenExpiryTime();

        // Update citizen with new refresh token
        citizen.RefreshToken = newRefreshToken;
        citizen.RefreshTokenExpiryTime = refreshTokenExpiry;
        citizen.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new RefreshResponse(
            accessToken,
            newRefreshToken,
            refreshTokenExpiry
        );
    }
}
