using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Auth.Me;

public sealed class MeQueryHandler : IRequestHandler<MeQuery, ErrorOr<MeResponse>>
{
    private readonly ApplicationDbContext _context;

    public MeQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<MeResponse>> Handle(
        MeQuery request,
        CancellationToken cancellationToken)
    {
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

        if (citizen == null)
        {
            return Error.NotFound(
                code: "Auth.UserNotFound",
                description: "User not found");
        }

        return new MeResponse(
            citizen.Id,
            citizen.FirstName,
            citizen.LastName,
            citizen.Email,
            citizen.PhoneNumber,
            citizen.NationalId,
            citizen.DateOfBirth,
            citizen.Address,
            citizen.TenantId,
            citizen.CreatedAt,
            citizen.LastLoginAt,
            citizen.IsActive
        );
    }
}
