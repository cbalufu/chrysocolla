using CitizensPortal.Api.Infrastructure.Authentication;
using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Auth.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(ApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ErrorOr<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // Find citizen by email (tenant context is automatically applied by global query filter)
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Email == request.Email, cancellationToken);

        if (citizen == null)
        {
            return Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "Invalid email or password");
        }

        // Verify password using BCrypt
        if (!BCrypt.Net.BCrypt.Verify(request.Password, citizen.PasswordHash))
        {
            return Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "Invalid email or password");
        }

        // Check if citizen is active
        if (!citizen.IsActive)
        {
            return Error.Forbidden(
                code: "Auth.AccountInactive",
                description: "Your account has been deactivated");
        }

        // Generate tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(
            citizen.Id,
            citizen.Email,
            citizen.TenantId);

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiry = _jwtTokenService.GetRefreshTokenExpiryTime();

        // Update citizen with refresh token
        citizen.RefreshToken = refreshToken;
        citizen.RefreshTokenExpiryTime = refreshTokenExpiry;
        citizen.LastLoginAt = DateTime.UtcNow;
        citizen.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            accessToken,
            refreshToken,
            refreshTokenExpiry,
            new CitizenInfo(
                citizen.Id,
                citizen.FirstName,
                citizen.LastName,
                citizen.Email,
                citizen.TenantId
            )
        );
    }
}
